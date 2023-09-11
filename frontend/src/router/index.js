import Vue from 'vue'
import VueRouter from 'vue-router'
import RegisterPage from "@/components/RegisterUser/RegisterPage.vue";
import LoginPage from "@/components/RegisterUser/LoginPage.vue";

Vue.use(VueRouter)

const routes = [
  {
    path: '/',
    name: 'home',
  },
  {
    path: '/registration',
    name: 'registration',
    component: RegisterPage,
  },
  {
    path: '/login',
    name: 'login',
    component: LoginPage
  }
]

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes
})

export default router
