<template>
  <div class="">
      <h1>Mina Listor</h1> <br>
      <div v-if="responseIfError !== 'empty'">
        <div class="ContainerDiv">
                <div class="listContainer" v-for="obj in responseContainerList1" :key="obj['title']">
                <ul>
                <div class="ListItemDiv"><input type="radio" @click="delete1(obj['id'])" /><th>{{obj['title']}}</th></div>
                    <li><div class="ListItemDiv" v-for="item in obj['listItem']" :key="item['id']"><input type="radio" @click="deleteItem(item['id'])" /><p  @click="item['completed'] = CompleteListItem(item['completed'])" v-bind:class="{ 'line-trough': item['completed']}">{{item['listItemText']}}</p></div></li>
                </ul>
                </div>
        </div>
        <div class="ContainerDivButton">
            <button class="buttonBlue" @click="get1()">Hämta listor</button> <br>
            <button class="buttonBlue" @click="saveAttempt()">Spara</button> <br>
        </div>
      </div>
        <router-link class="buttonBlue" v-else to="/create">Skapa nya listor</router-link>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import state from 'vuex';
import {List1, ListItem1} from '@/models/types';

@Component
export default class Api extends Vue {
    mounted() {
        this.get1();
    };

        items= "null";
        inputValue= "this is input";
        inputList= 'test';
        list= this.$store.state.z_string;
        input1= "";
        input2= "";
        userid= 0;
        listid= 0;
        responseContainerList1 = new List1;
        responseIfError = "";
        debug= false;
        linetrough = "line-trough";

        CompleteListItem(item: boolean) {
            console.log("here, also " + item);
            if(item === false)
                return true;
            else
               return false;
        };

        get1() {
          var self = this;
          fetch('https://localhost:44366/api/values/')
            .then(function(response) {
                if(response.status !== 200)
                    return self.responseIfError = "empty"
            return response.json();
            })
            .then(function(myJson) {
                self.items = JSON.stringify(myJson, null, 2);
                self.responseContainerList1 = myJson;
                self.$store.state.z_string = myJson;
                console.log(JSON.stringify(myJson, null, 2));
            });
        };

        get2() {
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
         };

        
        saveAttempt() {

            var self = this;
            console.log(self.inputList);
            console.log(self.responseContainerList1)
          fetch('https://localhost:44366/api/values/save/', {
              method: 'POST',
              body: JSON.stringify(self.responseContainerList1),
              headers: {'Content-Type': 'application/json'}});
          };

        getAttempt() {

                var self = this;
                self.inputList = self.$store.state.test;
                console.log(self.inputList);

                fetch('https://localhost:44366/api/values/new' + '/' + self.listid + '/' + self.userid, {
                    method: 'GET',
                    headers: {'Content-Type': 'application/json'}
                }).then(function(response) {
                    return response.json();
                    }).then(function(myJson) {
                        self.items = myJson;
                        console.log(JSON.stringify(myJson));
                    });
            };

        postAttempt2() {

                var self = this;
                var bodyContent = {ListId: self.listid, UserId: self.userid};
                console.log(self.inputList);

            fetch('https://localhost:44366/api/values/newPost', {
                method: 'POST',
                body: JSON.stringify(bodyContent),
                headers: {'Content-Type': 'application/json'}
                }).then(function(response) {
                return response.json();
                }).then(function(myJson) {
                    self.items = myJson;
                    console.log(JSON.stringify(myJson));
                });
            };

        post1() {

                var self = this;

                fetch('https://localhost:44366/api/values/', {
                    method: 'POST',
                    body: JSON.stringify(self.inputValue),
                    headers: {'Content-Type': 'application/json'}
                });
            };

        post2() {

                var self = this;
                
                fetch('https://localhost:44366/api/values/', {
                method: 'POST',
                body: JSON.stringify(self.inputValue),
                headers: {
                    'Content-Type': 'application/json'
                },
                }).then(function(response) {
                return response;
                }).then(data => {
                console.log(data);
                })
            };

        put1() {
                var self = this;
                fetch('https://localhost:44366/api/values/5', 
                {
                method: 'PUT',
                body: JSON.stringify(self.inputValue),
                headers: {
                    'Content-Type': 'application/json'
                },
                }).then(function(response) {
                return response;
                }).then(data => {
                console.log(data);
                })
            };
        delete1(id: number) {
                var self = this;
                fetch('https://localhost:44366/api/values/' + id, 
                {
                    method: 'DELETE',
                    headers: {
                        'Content-Type': 'application/json'
                },
                }).then(function(response) {
                    console.log(response.status)
                if(response.status !== 200)
                    return self.responseIfError = "empty"
                return response.json();
                }).then(function(myJson) {
                self.items = JSON.stringify(myJson, null, 2);
                if(self.responseIfError !== "empty") 
                    self.responseContainerList1 = myJson;
                self.$store.state.z_string = myJson;
            });
            };
        deleteItem(id: number) {
            var self = this;
            fetch('https://localhost:44366/api/values/DeleteItem/' + id, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json'
                },
            }).then(function(response) {
                if(response.status !== 200)
                     return self.responseIfError = "empty"
                else
                    return response.json();
            }).then(function(myJson) {
                self.items = JSON.stringify(myJson, null, 2);
                if(self.responseIfError !== "empty")
                    self.responseContainerList1 = myJson;
                self.$store.state.z_string = myJson;
                console.log(JSON.stringify(myJson, null, 2));
            });
        };

}
</script>

<style scoped>
.listContainer {
    display: inline;
    max-width: 200px;
    min-width: 100px;
    text-align: left;
    border: 2px solid orange;
}
.listContainer button {
    display: inline;
    background-color: red;
}
.listContainer ul {
    font-size: 2em;
    min-width: 150px;
    justify-content: space-around;
}
.listContainer li {
    padding: 4px;
    list-style-type: none;
}
.ListItemDiv p {
    display: inline;
}
.ListItemDiv th {
    display: inline;
}
.line-trough{
    text-decoration: line-through;
}
.ContainerDiv{
    border: 2px solid #3bb273;
    display: flex;
    justify-content: space-around;
    margin: 40px;    
}
.buttonBlue {
    background-color: #3bb273; /* Green */
    border: none;
    color: white;
    padding: 15px 32px;
    text-align: center;
    text-decoration: none;
    display: inline-block;
    font-size: 16px;
    margin: 4px;
    margin-left: 12px;
    }
.ContainerDivButton {
    display: flex;
    justify-content: space-around;
    margin-left: 40%;
    width: 20%;
    height: 70px;
    text-align: center;

}
.buttonsToBeSorted {
    margin-top: 400px;
    max-width: 500px;
}

ul li .check{
  display: block;
  position: absolute;
  border: 5px solid #AAAAAA;
  border-radius: 100%;
  height: 25px;
  width: 25px;
  top: 30px;
  left: 20px;
	z-index: 5;
	transition: border .25s linear;
	-webkit-transition: border .25s linear;
}

</style>

