import { HubConnectionBuilder, HubConnection, LogLevel } from '@microsoft/signalr'

export abstract class SignalRClientFactory {
  private static hubConnection: HubConnection

  private static createHubConnection(): void {
    const hubUrl = '/pingHub'
    
    SignalRClientFactory.hubConnection = new HubConnectionBuilder()
      .withUrl(hubUrl)
      .configureLogging(LogLevel.Information)
      .withAutomaticReconnect()
      .build()
  }

  public static get planboardHub(): HubConnection {
    if (!this.hubConnection) {
      this.createHubConnection()
    }
    return this.hubConnection
  }
}
