import Vue from 'vue'
import Vuex from 'vuex'
import auth from "@/store/models/auth";
import user from "@/store/models/user";

Vue.use(Vuex)

export default new Vuex.Store({
    modules: {
        auth, user
    }
})
