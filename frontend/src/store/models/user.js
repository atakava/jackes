import axios from "axios";

export default {
    namespaced: true, state: {
        user: {}
    }, getters: {
        currentUser: state => state.user
    }, mutations: {
        setUser(state, user) {
            state.user = user;
        }
    }, actions: {
        async fetchUserInfo({commit}) {
            try {
                const response = await axios.get('http://localhost:5273/api/User/current-user', {withCredentials: true});
                commit('setUser', response.data);
            } catch (e) {
                console.error("Ошибка текущего пользователя", e.response);
            }
        }, async editUserProfile({commit}, payload) {
            try {
                const response = await axios.post(`http://localhost:5273/api/User/update-user${payload.id}`, payload.userData, {
                    withCredentials: true, headers: {
                        'Content-Type': 'multipart/form-data'
                    }
                });
                console.log("Редактирование профиля" + response.data);
                commit('setUser', response.data);
            } catch (e) {
                console.error(e)
            }
        }
    }
}