<template>
  <div class="">
      <h1>Mina Listor</h1> <br>
      <div v-if="responseIfError !== 'empty'">
          <button @click="rerender()">Rerender list</button>
        <select class="select-css" @change="presentlist($event.target.selectedOptions[0]._value)" name="dropdown" id="dropdown">
            <option selected hidden value="empty">Välj Lista</option>
            <option v-for="obj in responseContainerList1" :key="obj['id']" :value="obj">{{obj['title']}}</option>
        </select>
      </div>
      <div v-if="responseIfError !== 'empty' && List_present !== 'empty'">
          <div v-if="List_present['UserId'] !== 0" class="ContainerDiv">
            <div class="listContainer">
                <font-awesome-icon class="editButton" @click="edit = CompleteListItem(edit)" :icon="['fas', 'edit']" ></font-awesome-icon>
                <ul>
                <div class="ListItemDiv">
                    <font-awesome-icon class="titleIcon" v-show="edit" type="radio" @click="delete1(List_present['id'])" :icon="['far', 'trash-alt']"></font-awesome-icon>
                    <th>{{List_present['title']}}</th>
                </div>
                    <li>
                        <div class="ListItemDiv" v-for="item in List_present['listItem']" :key="item['id']">
                            <div class="div1">
                                <font-awesome-icon class="itemIcon" v-show="edit" @click="deleteItem(item['id'])" :icon="['fas', 'times-circle']"></font-awesome-icon>
                                <font-awesome-icon v-bind:class="{ 'competedIconDone': item['completed'], 'completedIcon': true}" @click="item['completed'] = CompleteListItem(item['completed']), saveItem(item)" :icon="['fas', 'check']"></font-awesome-icon>
                            </div>
                            <div class="div2">
                                <p @click="item['completed'] = CompleteListItem(item['completed']), saveItem(item)" v-bind:class="{ 'completed': item['completed']}">{{item['listItemText']}}</p>
                            </div>
                        </div>
                    <input v-show="edit" @keypress.enter="AddItem($event.target.value)" class="addItem" placeholder="Skriv syssla eller sak"/>
                    </li>
                </ul>
            </div>
        </div>
      </div>
        <router-link class="buttonBlue" v-if="responseIfError === 'empty'" to="/create">Skapa nya listor</router-link>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import state from 'vuex';
import {List1, ListItem1} from '@/models/types';
import { library, config } from '@fortawesome/fontawesome-svg-core';
import { faTimesCircle, faEdit, faCheck} from "@fortawesome/free-solid-svg-icons";
import { faTrashAlt,faCheckCircle} from "@fortawesome/free-regular-svg-icons";
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';

library.add(faTrashAlt, faTimesCircle, faEdit, faCheckCircle, faCheck );
Vue.component('font-awesome-icon', FontAwesomeIcon);


@Component
export default class Api extends Vue {
    mounted() {
        this.get1();
        console.log("mounted!!");
    };
    updated() {
      if(this.update === 1) {
        console.log("updated!");
        this.get1();
        console.log("updated, update: " + this.update)
        this.update = 0;
      }
   
    };
        firstrun = 1;
        update = 0;
        edit = false;
        dropdown = "Välj Lista";
        items= "null";
        inputValue= "this is input";
        inputList= 'test';
        input1= "";
        input2= "";
        userid= 0;
        listid= 0;
        responseContainerList1 = new List1;
        responseIfError = "";
        debug= false;
        List_present = new List1;
        debug2 = "";
        selectedlist = "";
        event = "";

        CompleteListItem(item: boolean) {
            if(item === false){
                console.log("if, also " + item);
                return true;
            }
            else{
                console.log("else, also " + item);                
                return false;
            }
        };

        rerender() {
         this.update = 1;
        console.log("rerender, update: " + this.update)
        };

        presentlist(event: any){
            this.userid = event.userId;
            this.listid = event.listId;
            this.selectedlist = event;
                console.log(event);
            this.List_present = event;
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
                // console.log("get1 log:::::: " + JSON.stringify(myJson, null, 2));
                // console.log("GET1 = selectedOptions:::: " + JSON.stringify(self.selectedlist, null, 2));
                var newList: any;
                myJson.forEach(element => {
                    if(element.id === self.List_present.id)
                        newList = element;
                    console.log(element);
                });
                console.log("newlist " + JSON.stringify(newList));
                self.items = JSON.stringify(myJson, null, 2);
                self.responseContainerList1 = myJson;
                self.$store.state.z_string = myJson;
                console.log("before firstrun: " + self.firstrun)
                console.log("get before condition, update: " + self.update)
                if(self.firstrun === 0){
                    console.log("updates!!!");
                    console.log("PUSH!!!")
                    self.List_present = newList;
                    self.List_present.listItem.sort();
                    console.log("responselist_:::::::::::::::::::" + self.responseContainerList1);
                    self.responseContainerList1 = self.responseContainerList1;
                    self.update = 0;
                    console.log("last row inside loop, update: " + self.update)
                }
                self.firstrun = 0;
                console.log("after firstrun: " + self.firstrun)
                console.log("after loop, update: " + self.update)
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
                // console.log(JSON.stringify(myJson));
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

          saveItem(listitem: ListItem1) {

          fetch('https://localhost:44366/api/values/SaveItem/', {
              method: 'POST',
              body: JSON.stringify(listitem),
              headers: {'Content-Type': 'application/json'}});
          };

          AddItem(ListItemText: string) {
              var self = this;
              self.event = ListItemText;
              var bodyContent = {ListId: self.listid, UserId: self.userid, ListItemText: ListItemText};
          fetch('https://localhost:44366/api/values/AddItem/', {
              method: 'POST',
              body: JSON.stringify(bodyContent),
              headers: {'Content-Type': 'application/json'}
              });
              self.update = 1;
            //   console.log("before:::: " + JSON.stringify(self.List_present));
              self.List_present['listItem'].sort();
            //   console.log("after::::::" + JSON.stringify(self.List_present));
            //   console.log("ADDITEM = selectedOptions:::: " + JSON.stringify(self.selectedlist, null, 2));
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
                        console.log("myJson: " + JSON.stringify(myJson));
                    });
                    var items = JSON.stringify("self.items: " + self.items);
                    var list = new List1;
                    console.log(self.items);
                    console.log(items);

                    return self.items;
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
                // console.log(data);
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
                // console.log(data);
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
                if(self.responseIfError !== "empty") {
                    self.List_present['UserId'] = 0;
                    self.responseContainerList1 = myJson;
                }
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
                if(self.responseIfError !== "empty"){
                    
                    for (let index = 0; index < self.List_present['listItem'].length; index++) {
                        if(self.List_present['listItem'][index]['id'] === id)
                            self.List_present['listItem'].splice(index, 1);  
                    };
                    self.responseContainerList1 = myJson;
                }
                self.$store.state.z_string = myJson;
            });
        };

}
</script>

<style scoped>
.div1 {
    float: left;
}
.div2{
    margin: 25px;
    word-wrap: break-word;
}
.div2 p {
    margin-left: 25px;
}

.listContainer {
    padding: 10px;
    min-width: 100px;
    max-width: 800px;
    text-align: left;
}
.listContainer ul {
    min-width: 150px;
    justify-content: space-around;
}
.listContainer li {
    padding: 4px;
    list-style-type: none;
}
.ListItemDiv th {
    display: inline;
    font-size: 1.5em;
}
.ListItemDiv {
    line-height: 20px;
}
.ListItemDiv p {
    padding-left: 20px;
}
.ListItemDiv span {
    position: inline;
}
.completed{
    text-decoration: line-through;
}
.ContainerDiv{
    border: 2px solid #3bb273;
    margin: 40px;
    min-width: 500px;
    max-width: 80%;
}

.select-css {
    display: block;
    font-size: 16px;
    font-family: sans-serif;
    font-weight: 700;
    color: #444;
    line-height: 1.3;
    padding: .6em 1.4em .5em .8em;
    width: 100%;
    max-width: 30%; 
    box-sizing: border-box;
    margin-left: 35%;
    border: 1px solid #aaa;
    box-shadow: 0 1px 0 1px rgba(0,0,0,.04);
    border-radius: .5em;
    -moz-appearance: none;
    -webkit-appearance: none;
    appearance: none;
    background-color: #fff;
    background-image: url('data:image/svg+xml;charset=US-ASCII,%3Csvg%20xmlns%3D%22http%3A%2F%2Fwww.w3.org%2F2000%2Fsvg%22%20width%3D%22292.4%22%20height%3D%22292.4%22%3E%3Cpath%20fill%3D%22%23007CB2%22%20d%3D%22M287%2069.4a17.6%2017.6%200%200%200-13-5.4H18.4c-5%200-9.3%201.8-12.9%205.4A17.6%2017.6%200%200%200%200%2082.2c0%205%201.8%209.3%205.4%2012.9l128%20127.9c3.6%203.6%207.8%205.4%2012.8%205.4s9.2-1.8%2012.8-5.4L287%2095c3.5-3.5%205.4-7.8%205.4-12.8%200-5-1.9-9.2-5.5-12.8z%22%2F%3E%3C%2Fsvg%3E'),
      linear-gradient(to bottom, #ffffff 0%,#e5e5e5 100%);
    background-repeat: no-repeat, repeat;
    background-position: right .7em top 50%, 0 0;
    background-size: .65em auto, 100%;
}

.select-css:hover {
    border-color: #888;
}
.select-css:focus {
    border-color: #aaa;
    box-shadow: 0 0 0 3px -moz-mac-focusring;
    color: #222; 
    outline: none;
}
.select-css option {
    font-weight: normal;
}

body {
  padding: 3rem;
}
.titleIcon{
    font-size: 18px;
    padding-right: 8px;
}
.itemIcon {
    font-size: 15px;
    padding-right: 8px;
}
.itemIcon:hover {
    color:red;
    font-size: 1.1em;
}
.titleIcon:hover {
    color:red;
}
.editButton {
    font-size: 25px;
    padding: 5px;
}
.editButton:hover {
    color: #3bb273;
}
.completedIcon:hover {
    color: #3bb273;
}
.completedIcon {
    border: #3bb273 solid 1px;
    border-radius: 50%;
    font-size: 15px;
    margin-right:5px;
    padding: 5px;
}
.competedIconDone {
    border: #3bb273 solid 1px;
    background-color: #3bb273;
    border-radius: 50%;
    padding: 5px;
    color: white;
}
.listItemContainer {
    margin: 12px;
}
.addItem {
    min-width: 80%;
    word-wrap: break-word;
    display: inline;
    margin-left: 15%;
}
</style>

