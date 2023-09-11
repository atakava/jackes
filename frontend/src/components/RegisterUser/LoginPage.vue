<script>
import {mapActions} from "vuex";

export default {
  name: "LoginPage",
  data() {
    return {
      login: '',
      password: ''
    }
  },
  methods: {
    ...mapActions({
      loginUser: "auth/loginUser"
    }),
    async fetchLogin() {
      try {
        const response = await this.loginUser({
          login: this.login,
          password: this.password
        });
        this.login
        console.log(response);
      } catch (e) {
        console.error(e.response ? e.response.data : e);

      }
    }
  }
}
</script>

<template>
  <v-main>
    <div
        class="w-100 h-100 d-flex justify-content-center align-items-center"
    >
      <v-card max-width="400"
              class="w-100 pr-5 pl-5 pb-5 pt-5"
      >
        <v-card-title>
          Регистрация
        </v-card-title>
        
        <v-form @submit.prevent="fetchLogin">
          <v-text-field
              class="mr-10 ml-10"
              v-model="login"
              label="login"
          ></v-text-field>
          <v-text-field
              class="mr-10 ml-10"
              v-model="password"
              label="password"
          ></v-text-field>
          <v-btn
              type="submit"
              block
              color="primary"
              class="mb-7"
          >Войти
          </v-btn>
        </v-form>
        <div class="d-flex justify-content-center align-items-center">
          <p>нет аккаунта?
            <router-link to="registration">Войти</router-link>
          </p>
        </div>
      </v-card>
    </div>
  </v-main>
</template>

<style scoped>

</style>