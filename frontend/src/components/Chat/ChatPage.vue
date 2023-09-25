<template>
  <div>
    <div v-for="message in messages" :key="message.id">
      {{ message.user }}: {{ message.text }}
    </div>
    <input v-model="newMessage" @keyup.enter="sendMessage" placeholder="Введите сообщение">
  </div>
</template>

<script>
import * as signalR from "@microsoft/signalr"
import {mapActions} from "vuex";
export default {
  data() {
    return {
      hubConnection: null,
      messages: [],
      newMessage: "",
    }
  },
  computed: {
    currentUser() {
      return this.$store.getters["user/currentUser"];
    } 
  },
  async mounted() {
    await this.fetchUserInfo();
    this.initSignalR();
    console.log(this.currentUser);  
  },
  methods: {
    ...mapActions({
      fetchUserInfo: 'user/fetchUserInfo'
    }),
    initSignalR() {
      this.hubConnection = new signalR.HubConnectionBuilder()
          .withUrl("http://localhost:5273/chat", {
            skipNegotiation: true,
            transport: signalR.HttpTransportType.WebSockets
          }).build();
      
      this.hubConnection.onreconnected(() => {
        this.sendMessage();
      });
      
      this.hubConnection.on("OnMessage", (data) => {
        console.log(data)
        this.messages.push(data.message);
      });
      
      this.hubConnection.on("OnConnected", (data) => {
        this.messages = data.items;
      });
      
      this.hubConnection.start().catch((error) => {
        console.error("Error start SignalR connection:", error);
      });
    },
    sendMessage() {
      if(this.newMessage) {
        const text = this.newMessage;
        const user = this.currentUser.login;
        this.hubConnection.invoke("OnMessage", text, user).catch((error) => {
          console.error("Error send message", error);
        });
        this.newMessage = "";
      }
    }
  }
}
</script>

<style scoped>
</style>
