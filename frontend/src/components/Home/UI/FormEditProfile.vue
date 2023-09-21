<script>
import {mapActions} from "vuex";

export default {
  name: "FormEditProfile",
  props: {
    id: Number,
    user: {}
  },
  data() {
    return {
      login: this.user.login,
      mail: this.user.mail,
      role: this.user.role,
      password: '',
      validPass: false,
      avatar: null,
      edit: true,
      saveText: 'Изменить'
    }
  },
  methods: {
    ...mapActions(
        {
          editUserProfile: 'user/editUserProfile'
        }
    ),
    saveEdit() {
      this.edit = !this.edit;

      if (this.edit == true)
        this.saveText = "Изменить";

      else
        this.saveText = "Сохранить";
    },
    onChange(e) {
      this.avatar = e.target.files[0];
    },
    async editUser() {
      const formData = new FormData();

      if (typeof (this.login) === "string" && this.login.length > 0)
        formData.append("Login", this.login);

      if (typeof (this.mail) === "string" && this.mail.length > 0)
        formData.append("Mail", this.mail);

      formData.append("Avatar", this.avatar);

      console.log(this.avatar);

      try {
        await this.editUserProfile({userData: formData, id: this.id});
        this.login = '';
        this.mail = '';
        this.avatar = null;
      } catch (e) {
        console.log(e)
      }
    }
  }
}
</script>

<template>
  <div>
    <v-form @submit.prevent="editUser">
      <v-text-field
          v-model="login"
          label="login"
          :disabled="edit"
      ></v-text-field>
      <v-text-field
          v-model="mail"
          label="mail"
          :disabled="edit"
      ></v-text-field>
      <v-text-field
          v-model="role"
          label="role"
          :disabled="edit"
      ></v-text-field>
      <input type="file" :disabled="edit" @change="onChange">
      <v-btn
          @click="saveEdit"
      >
        {{ saveText }}
      </v-btn>

      <v-btn
          type="submit"
          color="primary"
      >
        Обновить
      </v-btn>
    </v-form>
  </div>
</template>

<style scoped>

</style>