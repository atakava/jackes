import Vue from 'vue'
import VueRouter from 'vue-router'
import RegisterPage from "@/components/RegisterUser/RegisterPage.vue";
import LoginPage from "@/components/RegisterUser/LoginPage.vue";
import TestPAge from "@/components/Home/TestPAge.vue";
import HomePage from "@/components/Home/HomePage.vue";
import ChatPage from "@/components/Chat/ChatPage.vue";

Vue.use(VueRouter)

const routes = [
    {
        path: '/registration',
        name: 'registration',
        component: RegisterPage,
    },
    {
        path: '/login',
        name: 'login',
        component: LoginPage
    },
    {
        path: '/test',
        name: 'test',
        component: TestPAge
    },
    {
        path: '/home',
        name: 'home',
        component: HomePage
    },
    {
        path: '/chat',
        name: 'chat',
        component: ChatPage
    }
]

const router = new VueRouter({
    mode: 'history',
    base: process.env.BASE_URL,
    routes
})

export default router
