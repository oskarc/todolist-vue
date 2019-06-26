import Vue from 'vue';
import App from './app.vue';
import VueRouter from 'vue-router';
import Create from './Create.vue';
import Home from './Home.vue';
import store from './Store/store.js';

Vue.use(VueRouter);

const routes = [
    { path: '/create', component: Create},
    { path: '/', component: Home}
];
const router = new VueRouter({
  routes,
  mode: 'history'
});

new Vue({
  el: '#app',
  data() {
    return {
    }
  },
  router,
  store,
  render: h => h(App),
}).$mount('#app');
