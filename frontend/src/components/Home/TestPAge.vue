<script>
import {mapActions} from "vuex";

export default {
  name: "TestPAge",
  methods: {
    ...mapActions({
      fetchUserInfo: "user/fetchUserInfo"
    }),
  },
  computed: {
    user() {
      return this.$store.getters["user/currentUser"];
    }
  },
  async mounted() {
    await this.fetchUserInfo();
    console.log(this.user)
  }
}
</script>

<template>
  <v-main>
    <v-card>
      <v-card-title>
        login: {{ user.login }}
      </v-card-title>
      <v-card-text>
        Id: {{ user.id }} Password: {{ user.password }} Mail: {{ user.mail }} Role: {{ user.role }}
      </v-card-text>
    </v-card>
    <div>
      <v-card v-for="item in user.posts" :key="item.id">
        id:{{ item.id }}
        title: {{ item.title }}
        text: {{ item.text }}
        <v-card-text v-for="comment in item.comments" :key="comment.id">
          text: {{ comment.text }}
        </v-card-text>
      </v-card>
    </div>

    <div>
      <v-card v-for="item in user.comments" :key="item.id">
        text: {{ item.text }}
      </v-card>
    </div>
  </v-main>
</template>