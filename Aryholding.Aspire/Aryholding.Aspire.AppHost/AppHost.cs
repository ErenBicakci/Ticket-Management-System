var builder = DistributedApplication.CreateBuilder(args);

var postgresUser = builder.AddParameter("postgres-user", "ticketsystemdb");
var postgresPassword = builder.AddParameter("postgres-password", secret: true);
var jwtKey = builder.AddParameter("jwt-key", secret: true);
var grafanaAdminPassword = builder.AddParameter("grafana-admin-password", secret: true);

var postgres = builder.AddPostgres("postgres", postgresUser, postgresPassword)
    .WithImageTag("16")
    .WithDataVolume("tms-postgres-data")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithHostPort(5444);

var devConnection = postgres.AddDatabase("DevConnection", "ticketsystemdb");

var redis = builder.AddContainer("redis", "redis", "7.2-alpine")
    .WithArgs("redis-server", "--appendonly", "yes")
    .WithVolume("tms-redis-data", "/data")
    .WithEndpoint(port: 6379, targetPort: 6379, name: "tcp", scheme: "tcp")
    .WithLifetime(ContainerLifetime.Persistent);

var redisEndpoint = redis.GetEndpoint("tcp");

var kafka = builder.AddKafka("kafka", port: 9092)
    .WithDataVolume("tms-kafka-data")
    .WithLifetime(ContainerLifetime.Persistent);

var elasticsearch = builder.AddContainer("elasticsearch", "docker.elastic.co/elasticsearch/elasticsearch", "8.17.2")
    .WithEnvironment("discovery.type", "single-node")
    .WithEnvironment("xpack.security.enabled", "false")
    .WithEnvironment("ES_JAVA_OPTS", "-Xms1g -Xmx1g")
    .WithVolume("tms-elasticsearch-data", "/usr/share/elasticsearch/data")
    .WithHttpEndpoint(port: 9200, targetPort: 9200, name: "http")
    .WithLifetime(ContainerLifetime.Persistent);

var elasticsearchHttp = elasticsearch.GetEndpoint("http");

var kibana = builder.AddContainer("kibana", "docker.elastic.co/kibana/kibana", "8.17.2")
    .WithEnvironment("ELASTICSEARCH_HOSTS", elasticsearchHttp)
    .WithHttpEndpoint(port: 5601, targetPort: 5601, name: "http")
    .WaitFor(elasticsearch);

var dashboardOtlpApiKey = builder.Configuration["AppHost:OtlpApiKey"] ?? "";

var otelCollector = builder.AddContainer("otel-collector", "otel/opentelemetry-collector-contrib", "0.114.0")
    .WithBindMount("../../infrastructure/otel/otel-collector-config.yaml", "/etc/otelcol-contrib/config.yaml", isReadOnly: true)
    .WithEnvironment("ASPIRE_OTLP_API_KEY", dashboardOtlpApiKey)
    .WithOtlpExporter()
    .WithHttpEndpoint(port: 4318, targetPort: 4318, name: "otlp-http")
    .WithEndpoint(port: 4317, targetPort: 4317, name: "otlp-grpc", scheme: "http");

var ticketManagement = builder.AddProject<Projects.Aryholding_Tms_TicketManagement>("ticketmanagement")
    .WithReference(devConnection)
    .WithReference(kafka)
    .WithEnvironment("JWT__Key", jwtKey)
    .WaitFor(devConnection)
    .WaitFor(kafka);

var authService = builder.AddProject<Projects.Aryholding_Tms_AuthService>("authservice")
    .WithReference(devConnection)
    .WithReference(kafka)
    .WithEnvironment("JWT__Key", jwtKey)
    .WaitFor(devConnection)
    .WaitFor(kafka)
    .WaitFor(ticketManagement);

var generalService = builder.AddProject<Projects.Aryholding_Tms_GeneralService>("generalservice")
    .WithReference(devConnection)
    .WithEnvironment("ConnectionStrings__redis", redisEndpoint.Property(EndpointProperty.HostAndPort))
    .WithReference(kafka)
    .WithEnvironment("JWT__Key", jwtKey)
    .WaitFor(devConnection)
    .WaitFor(redis)
    .WaitFor(kafka)
    .WaitFor(ticketManagement);

var authHttp = authService.GetEndpoint("http");
var generalHttp = generalService.GetEndpoint("http");
var ticketHttp = ticketManagement.GetEndpoint("http");

var apisix = builder.AddContainer("apisix", "apache/apisix", "3.18.0-debian")
    .WithBindMount("../../infrastructure/apisix/config.yaml", "/usr/local/apisix/conf/config.yaml", isReadOnly: true)
    .WithBindMount("../../infrastructure/apisix/apisix.yaml", "/usr/local/apisix/conf/apisix.yaml", isReadOnly: true)
    .WithEnvironment("AUTHSERVICE_HOST", authHttp.Property(EndpointProperty.Host))
    .WithEnvironment("AUTHSERVICE_PORT", authHttp.Property(EndpointProperty.Port))
    .WithEnvironment("GENERALSERVICE_HOST", generalHttp.Property(EndpointProperty.Host))
    .WithEnvironment("GENERALSERVICE_PORT", generalHttp.Property(EndpointProperty.Port))
    .WithEnvironment("TICKETSERVICE_HOST", ticketHttp.Property(EndpointProperty.Host))
    .WithEnvironment("TICKETSERVICE_PORT", ticketHttp.Property(EndpointProperty.Port))
    .WithHttpEndpoint(port: 9080, targetPort: 9080, name: "http")
    .WithEndpoint(port: 9443, targetPort: 9443, name: "https", scheme: "https")
    .WithEndpoint(port: 9091, targetPort: 9091, name: "metrics", scheme: "http")
    .WaitFor(authService)
    .WaitFor(generalService)
    .WaitFor(ticketManagement)
    .WaitFor(redis)
    .WaitFor(elasticsearch)
    .WaitFor(otelCollector);

var prometheus = builder.AddContainer("prometheus", "prom/prometheus", "latest")
    .WithBindMount("../../infrastructure/prometheus/prometheus.yml", "/etc/prometheus/prometheus.yml", isReadOnly: true)
    .WithHttpEndpoint(port: 9090, targetPort: 9090, name: "http")
    .WaitFor(apisix);

var grafana = builder.AddContainer("grafana", "grafana/grafana", "10.2.3")
    .WithEnvironment("GF_SECURITY_ALLOW_EMBEDDING", "true")
    .WithEnvironment("GF_SECURITY_ADMIN_USER", "admin")
    .WithEnvironment("GF_SECURITY_ADMIN_PASSWORD", grafanaAdminPassword)
    .WithVolume("tms-grafana-data", "/var/lib/grafana")
    .WithHttpEndpoint(port: 3000, targetPort: 3000, name: "http")
    .WithLifetime(ContainerLifetime.Persistent)
    .WaitFor(prometheus);

var frontend = builder.AddNpmApp("frontend", "../../Aryholding-Tms-FrontEnd/aryholding-tms-frontend", "serve")
    .WithHttpEndpoint(port: 8080, targetPort: 8080, name: "http", isProxied: false)
    .WaitFor(apisix);

builder.Build().Run();
