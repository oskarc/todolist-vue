<template>
  <div id="app">
    <div id="nav">
     <p>
        <router-link to="/">Home</router-link> |  
        <router-link to="/api">Mina Listor</router-link> |
        <router-link to="/create">Skapa Lista</router-link> |
        <router-link to="/">.</router-link> 
     </p> 
    </div>
    <img alt="Vue logo" src="../src/assets/logo.png">    
    <router-view/>
  </div>
</template>

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

