<script>
import {mapActions} from "vuex";

export default {
  name: "RegisterPage",
  data() {
    return {
      login: '',
      mail: '',
      password: '',
      responseError: ''
    }
  },
  methods: {
    ...mapActions({
      createUser: 'auth/createUser'
    }),
    async register() {
      try {
        if (this.login == "" || this.mail == "" || this.password == "") {
          return this.responseError = "Заполните поля ввода";
        } else {
          const response = await this.createUser({
            login: this.login,
            mail: this.mail,
            password: this.password
          });
          this.responseError = response || '';
          this.login = '';
          this.mail = '';
          this.password = '';
        }
      } catch (e) {
        console.error(e.message);
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
        <v-card-text>
          {{ responseError }}
        </v-card-text>
        <v-form @submit.prevent="register">
          <v-text-field
              class="mr-10 ml-10"
              v-model="login"
              label="login"
          ></v-text-field>
          <v-text-field
              class="mr-10 ml-10"
              v-model="mail"
              label="mail"
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
          <p>уже зарегестрированы?
            <router-link to="login">Войти</router-link>
          </p>
        </div>
      </v-card>
    </div>
  </v-main>
</template>

<style scoped>

</style>