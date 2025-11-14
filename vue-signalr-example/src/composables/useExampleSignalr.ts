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
  let intervalId: number | null = null

  function setupEventHandlers() {
    hubConnection.on('ReceivePing', (pingMessage: PingMessage) => {
      console.log('ReceivePing:', pingMessage)
      lastPing.value = pingMessage
    })
  }

  async function sendPing() {
    try {
      await hubConnection.invoke('PingFromFrontend', 'Ping from Vue client')
      // .send can also be used to send a message as fire and forget. .invoke expects a response (like RPC call)
      console.log('Ping sent to server')
    } catch (error) {
      console.error('Failed to send ping:', error)
    }
  }

  function startPinging() {
    intervalId = window.setInterval(() => {
      sendPing()
    }, 5000)
  }

  function cleanup() {
    hubConnection.off('ReceivePing')
    if (intervalId !== null) {
      clearInterval(intervalId)
      intervalId = null
    }
  }

  async function initialize() {
    setupEventHandlers()
    startPinging()
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
