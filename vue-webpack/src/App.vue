<template>
  <div id="app">
  <h1>Lets go somewhere!</h1>
  <hr>
  <span>{{this.$store.state.test}}</span>
  <router-link to="/">Home</router-link>
  <router-link to="/create">Skapa lista</router-link>
  <hr>
  <router-view></router-view>
  <!-- <Home></Home> -->
  </div>
</template>

<script>
import Create from './Create'
import Home from './Home'

export default {
  data () {
    return {
      
    }
  },
  components:{
    Create,
    Home
  }
}
</script>

<style>

</style>
