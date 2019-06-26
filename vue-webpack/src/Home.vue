<template>
<div>
    <h1>Mina listor</h1>
    <span>response: {{items}}</span> <br>
    <button v-on:click="get1()">Get list id's</button> <br>
    <button v-on:click="get2(input1)">Specify Listid</button> <input class="input1" v-model="input1" type="text"> <br>
    <!-- <button v-on:click="post1()">POST 1</button> <br>
    <button v-on:click="post2()">POST 2</button> <br> -->
    <button v-on:click="put1()">PUT 1</button> <br>
    <button v-on:click="delete1(input2)">DELETE</button> <input class="input1" v-model="input2" type="text"> <br>
    <button v-on:click="postAttempt()">Add list</button> <br>
    <button v-on:click="getAttempt()">getAttempt!</button> <input class="input1" type="text" v-model="userid" placeholder="userid"> <input class="input1" type="text" v-model="listid" placeholder="listid"> <br>
    <button v-on:click="postAttempt2()">postAttempt2!</button> <input class="input1" type="text" v-model="userid" placeholder="userid"> <input class="input1" type="text" v-model="listid" placeholder="listid">
    <span>{{list}}</span>
</div>
</template>

<script>
import CreateVue from './Create.vue';
import {myVar, settings} from './Globals.js'
export default {
    data: function() {
        return {
            items: "null",
            inputValue: "this is input",
            inputList: 'test',
            list: "",
            input1: "",
            input2: "",
            userid: 0,
            listid: 0
        }
    },
  methods: {
    get1: function() {
          var self = this;
          fetch('https://localhost:44366/api/values/')
            .then(function(response) {
            return response.json();
            })
            .then(function(myJson) {
                self.items = myJson;
                console.log(JSON.stringify(myJson));
            });
    },
    get2: function() {
          var self = this;
          fetch('https://localhost:44366/api/values/' + self.input1,{
              method: 'GET'
          })
            .then(function(response) {
            return response.json();
            })
            .then(function(myJson) {
                self.items = myJson;
                console.log(JSON.stringify(myJson));
            });
    },
    postAttempt: function() {
            var self = this;
            self.inputList = self.$store.state.test;
            console.log(self.inputList);
          fetch('https://localhost:44366/api/values/', {
              method: 'POST',
              body: JSON.stringify(self.inputList),
              headers: {'Content-Type': 'application/json'}
          });
    },
    getAttempt: function() {
            var self = this;
            self.inputList = self.$store.state.test;
            console.log(self.inputList);
          fetch('https://localhost:44366/api/values/new' + '/' + self.listid + '/' + self.userid, {
              method: 'GET',
              headers: {'Content-Type': 'application/json'}
          }).then(function(response) {
            return response.json();
            })
            .then(function(myJson) {
                self.items = myJson;
                console.log(JSON.stringify(myJson));
            });
    },
    postAttempt2: function() {
            var self = this;
            var bodyContent = {ListId: self.listid, UserId: self.userid};
            console.log(self.inputList);
          fetch('https://localhost:44366/api/values/newPost', {
              method: 'POST',
              body: JSON.stringify(bodyContent),
              headers: {'Content-Type': 'application/json'}
          }).then(function(response) {
            return response.json();
            })
            .then(function(myJson) {
                self.items = myJson;
                console.log(JSON.stringify(myJson));
            });
    },
    post1: function() {
            var self = this;
          fetch('https://localhost:44366/api/values/', {
              method: 'POST',
              body: JSON.stringify(self.inputValue),
              headers: {'Content-Type': 'application/json'}
          });
    },
    post2: function() {
            var self = this;
            fetch('https://localhost:44366/api/values/', {
            method: 'POST',
            body: JSON.stringify(self.inputValue),
            headers: {
                'Content-Type': 'application/json'
            },
            })
            .then(function(response) {
            return response;
            })
            .then(data => {
            console.log(data);
            })
    },
    put1: function() {
            var self = this;
            fetch('https://localhost:44366/api/values/5', 
            {
            method: 'PUT',
            body: JSON.stringify(self.inputValue),
            headers: {
                'Content-Type': 'application/json'
            },
            })
            .then(function(response) {
            return response;
            })
            .then(data => {
            console.log(data);
            })
    },
    delete1: function() {
            var self = this;
            fetch('https://localhost:44366/api/values/' + self.input2, 
            {
            method: 'DELETE',
            body: JSON.stringify(self.inputValue),
            headers: {
                'Content-Type': 'application/json'
            },
            })
            .then(function(response) {
            return response;
            })
            .then(data => {
            console.log(data);
            })
    },
    components: {
        
    },
    computed: {
       
    },
  }
}

</script>

<style scoped>
.input1 {
    max-width: 40px;
    
}
</style>
