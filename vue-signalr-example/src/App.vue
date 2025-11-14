<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { SignalRClientFactory } from '@/services/SignalrService'
import { useExampleSignalR } from '@/composables/useExampleSignalr'

const connectionStatus = ref<'connecting' | 'connected' | 'reconnecting' | 'disconnected'>('connecting')
const { initialize, lastPing } = useExampleSignalR()

function establishSignalRConnection() {
  const hubConnection = SignalRClientFactory.planboardHub

  hubConnection.onreconnected(() => {
    connectionStatus.value = 'connected'
    console.log('SignalR reconnected')
  })

  hubConnection.onreconnecting(() => {
    connectionStatus.value = 'reconnecting'
    console.log('SignalR reconnecting...')
  })

  hubConnection.onclose(() => {
    connectionStatus.value = 'disconnected'
    console.log('SignalR disconnected')
  })

  hubConnection
    .start()
    .then(() => {
      connectionStatus.value = 'connected'
      console.log('SignalR connection established')
      initialize()
    })
    .catch((error: unknown) => {
      connectionStatus.value = 'disconnected'
      console.error('SignalR connection error:', error)
    })
}

onMounted(() => {
  establishSignalRConnection()
})
</script>

<template>
  <div style="padding: 2rem; font-family: system-ui, sans-serif;">
    <h1>SignalR Ping Example</h1>
    
    <div style="margin: 1rem 0; padding: 1rem; border: 1px solid #ddd; border-radius: 4px;">
      <h2>Connection Status</h2>
      <p>
        <strong>Status:</strong> 
        <span :style="{ 
          color: connectionStatus === 'connected' ? 'green' : 
                 connectionStatus === 'reconnecting' ? 'orange' : 
                 connectionStatus === 'disconnected' ? 'red' : 'gray' 
        }">
          {{ connectionStatus.toUpperCase() }}
        </span>
      </p>
    </div>

    <div style="margin: 1rem 0; padding: 1rem; border: 1px solid #ddd; border-radius: 4px;">
      <h2>Last Ping</h2>
      <div v-if="lastPing">
        <p><strong>Message:</strong> {{ lastPing.message }}</p>
        <p><strong>Timestamp:</strong> {{ new Date(lastPing.timestamp).toLocaleString() }}</p>
      </div>
      <p v-else style="color: #666;">Waiting for pings from server...</p>
    </div>
  </div>
</template>

<style scoped></style>
