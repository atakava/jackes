<script>
export default {
  name: "PostList",
  props: {
    post: []
  }
}
</script>
<template>
  <v-main>
    {{ post}}
    <v-card v-for="item in">
      
    </v-card>
  </v-main>
</template>

<style scoped>

</style>