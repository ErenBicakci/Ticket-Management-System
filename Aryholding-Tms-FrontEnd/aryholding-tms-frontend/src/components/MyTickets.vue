<template>
  <div class="my-tickets-page">
    <TicketBoard
      :api-endpoint="'/api/ticket/my-tickets'"
      :additional-params="{}"
      :can-edit="true"
      :can-assign="false"
      :show-create-button="true"
      @create-ticket="createNewTicket"
      @assign-ticket="assignTicket"
      @ticket-updated="handleTicketUpdate"
    />
  </div>
</template>

<script>
import TicketBoard from './TicketBoard.vue'

export default {
  name: 'MyTickets',
  components: {
    TicketBoard
  },
  props: {
    userInfo: {
      type: Object,
      default: null
    }
  },
  methods: {
    createNewTicket() {
      this.$emit('create-ticket')
      if (this.$router) {
        this.$router.push('/tickets/new')
      }
    },
    
    assignTicket(ticketId) {
      console.log('Assigning ticket:', ticketId)
    },
    
    handleTicketUpdate(updatedData) {
      console.log('Ticket updated:', updatedData)
    }
  }
}
</script>

<style scoped>
.my-tickets-page {
  padding: 30px 0;
}
</style>
