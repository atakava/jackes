import axios from "axios";

export default {
    namespaced: true,
    state: {
        user: {}
    },
    getters: {
        userInfo(state) {
            return state.user;
        }
    },
    mutations: {
        setUser(state, user) {
            state.user = user;
        }
    },
    actions: {
        async createUser({commit}, userData) {
            try {
                const response = await axios.post(
                    "http://localhost:5273/api/User/register",
                    userData,
                    {withCredentials: true}
                );
                console.log("Регистрация прошла успешно");
                console.log(userData);
                commit('setUser', response.data);
                return null
            } catch (e) {
                console.error("Ошибка при регистрации " + e.response.data);
                return "Ошибка при регистрации " + e.response.data;
            }
        },
        async loginUser({commit}, userData) {
            try {
                const response = await axios.post(
                    "http://localhost:5273/api/User/login",
                    userData,
                    {withCredentials: true}
                );
                console.log("вы вошли успешно");
                console.log(userData);
                commit('setUser', response.data);
                return response.data
            } catch (e) {
                console.error("Ошибка при входе " + e.response.data);
                return "Ошибка при входе " + e.response.data;
            }
        }
    }
}