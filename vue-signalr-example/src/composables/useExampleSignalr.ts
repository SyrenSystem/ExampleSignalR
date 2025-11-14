import { onUnmounted, ref } from 'vue'
import type { HubConnection } from '@microsoft/signalr'
import { SignalRClientFactory } from '@/services/SignalrService'

interface PingMessage {
  timestamp: string
  message: string
}

export function useExampleSignalR() {
  const hubConnection: HubConnection = SignalRClientFactory.planboardHub
  const lastPing = ref<PingMessage | null>(null)

  function setupEventHandlers() {
    hubConnection.on('ReceivePing', (pingMessage: PingMessage) => {
      console.log('ReceivePing:', pingMessage)
      lastPing.value = pingMessage
    })
  }

  function cleanup() {
    hubConnection.off('ReceivePing')
  }

  async function initialize() {
    setupEventHandlers()
    console.log('SignalR ping listener initialized')
  }

  onUnmounted(() => {
    cleanup()
  })

  return {
    initialize,
    cleanup,
    lastPing
  }
}
