<template>
  <div id="app">
    <div id="nav">
      <!-- <p>debug: {{$store.state.isLoggedIn}}</p> -->
     <span>
        <!-- <router-link to="/">Home</router-link> | -->
        <span v-show="$store.state.isLoggedIn" ><router-link to="/api">Mina Listor</router-link> | </span>
        <span v-show="$store.state.isLoggedIn"><router-link to="/create">Skapa Lista</router-link> | </span>
        <span v-show="$store.state.isLoggedIn" ><router-link to="/preferences">Inställningar</router-link> | </span>
        <span v-show="!$store.state.isLoggedIn"><router-link to="/register">Registrera</router-link> | </span>
        <span><router-link to="/login">{{$store.state.Login}}</router-link></span>
        <!-- <router-link to="/">.</router-link>  -->
     </span> 
    </div>
    <img alt="Vue logo" src="../src/assets/logo.png">    
    <router-view/>
  </div>
</template>

<script lang="ts">
import Vue from 'vue'
import store from '@/store'
export default Vue.extend({
  data() {
    return {
      debug: store.state.isLoggedIn,
    }
  }
})
</script>

<style>
#app {
  font-family: 'Avenir', Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}
#nav {
  padding: 30px;
}

#nav a {
  font-weight: bold;
  color: #2c3e50;
}

#nav a.router-link-exact-active {
  color: #42b983;
}
#nav p {
  font-size: 1.5em;
}
</style>

