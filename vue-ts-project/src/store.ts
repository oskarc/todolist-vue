import Vue from 'vue';
import Vuex from 'vuex';
import {List1, ListItem1, User} from '@/models/types'


Vue.use(Vuex);

export default new Vuex.Store({
  state: {
    list: "default",
    z_string: "",
    z_arraystr: [],
    z_arrayarray: [[]],
    user: new User,
    isLoggedIn: false,
    Login: "Logga in"

  },
  mutations: {
  },
  actions: {
    
  },
});
