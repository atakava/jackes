<script>
import {mapActions} from "vuex";
import FormEditProfile from "@/components/Home/UI/FormEditProfile.vue";
import PostList from "@/components/Home/UI/PostList.vue";

export default {
  name: "HomePage",
  components: {PostList, FormEditProfile},
  methods: {
    ...mapActions({
      fetchUserInfo: 'user/fetchUserInfo'
    })
  },
  data() {
    return {
      visibleEdit: false
    }
  },
  computed: {
    user() {
      return this.$store.getters['user/currentUser'];
    }
  },
  async mounted() {
    await this.fetchUserInfo();
  }
}
</script>

<template>
  <div class="d-flex justify-content-sm-between">
    <v-navigation-drawer
        style="height: 100vh; position: fixed"
        expand-on-hover
        rail
    >
      a
    </v-navigation-drawer>
    <div class="d-flex flex-column justify-content-center align-items-center w-100">
      <v-card class="d-flex justify-content-sm-between align-items-center pl-5 pr-5">
        <v-img
            class="rounded-circle"
            width="70"
            height="70"
            :src="user.avatar"
        ></v-img>
        <div>
          <v-card-title>
            {{ user.login }}
          </v-card-title>
          <v-card-text>
            <div></div>
            <v-btn
                color="primary"
                block
                @click="visibleEdit = !visibleEdit"
            >
              профиль
            </v-btn>

            <v-dialog
                fullscreen
                v-model="visibleEdit"
                transition="dialog-bottom-transition"
            >
              <template>
                <v-card>
                  <v-card-title>
                    Профиль
                    <v-spacer></v-spacer>
                    <v-btn icon @click="visibleEdit = false">
                      <v-icon>mdi-close</v-icon>
                    </v-btn>
                  </v-card-title>
                  <v-card-text>
                    <FormEditProfile :user="user" :id="user.id"></FormEditProfile>
                  </v-card-text>
                </v-card>
              </template>
            </v-dialog>
          </v-card-text>
        </div>
      </v-card>
      <v-main>
        <PostList :post="user.posts"></PostList>
      </v-main>
    </div>
  </div>
</template>

<style scoped>
.dialog-bottom-transition-enter-active,
.dialog-bottom-transition-leave-active {
  transition: transform .2s ease-in-out;
}
</style>