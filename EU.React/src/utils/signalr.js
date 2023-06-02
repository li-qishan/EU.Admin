import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr'

const signalr = new HubConnectionBuilder()
.withUrl('/chat')
.configureLogging(LogLevel.Information)
.build();

export default signalr;
