<template>
  <div class="">
      <h1>Mina Inställningar</h1> <br>
      <div class="newCategory" v-if="responseIfError !== 'empty'">
        <p >Namnge Ny Kategori</p>
        <input type="text" v-model="CategoryTitle"/> <br>
        <select class="select-css" v-model="selected" @change="presentlist($event.target.value)" name="dropdown" id="dropdown">
            <option value="colorscheme1">colorscheme 1</option>
            <option value="colorscheme2">colorscheme 2</option>
            <option value="colorscheme3">colorscheme 3</option>
            <option value="colorscheme4">colorscheme 4</option>
            <option value="colorscheme5">colorscheme 5</option>
        </select>
        <button @click="AddUserSetting">Submit</button>
      </div>
      <div class="updateCategories" v-if="responseIfError !== 'empty'">
        <p>Ändra Färgschema på Kategori</p>
        <select class="select-css" @change="getCategoryColor($event.target.value)" name="dropdown" id="dropdown">
            <option selected hidden value="empty">Välj Lista</option>
            <option v-for="item in categories" :key="item['Id']" :value="item['category']">{{item['category']}}</option>
        </select>
        <select class="select-css" @change="ChangecolorScheme = $event.target.value" name="dropdown" id="dropdown">
            <option value="colorscheme1">colorscheme 1</option>
            <option value="colorscheme2">colorscheme 2</option>
            <option value="colorscheme3">colorscheme 3</option>
            <option value="colorscheme4">colorscheme 4</option>
            <option value="colorscheme5">colorscheme 5</option>
        </select>
        <button @click="UpdateColor()">Submit</button>
      </div>
      <div class="updateCategories" v-if="responseIfError !== 'empty'">
        <p>Byt Namn på Kategori</p>
        <select class="select-css" @change="getCategoryName($event.target.value)" name="dropdown" id="dropdown">
            <option selected hidden value="empty">Välj Lista</option>
            <option v-for="item in categories" :key="item['Id']" :value="item['category']">{{item['category']}}</option>
        </select>
        <input type="text" v-model="ChangeCategoryTitle"/> <br>
        <button @click="UpdateName()">Submit</button>
      </div>
      <div v-if="responseIfError !== 'empty'">
      <p>Mina Kategorier</p>
        <table>
            <tr>
            <th>Kategori</th>
            <th>Färgschema</th>
            </tr>
            <tr :key="item['Id']" v-for="item in categories">
                <td>{{item['category']}}</td>
                <td :class="item['colorScheme']">{{item['colorScheme']}}</td>
            </tr>
        </table>
      </div>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import state from 'vuex';
import {List1, ListItem1} from '@/models/types';
import { library, config } from '@fortawesome/fontawesome-svg-core';
import { faTimesCircle, faEdit, faCheck, faArrowUp, faArrowDown} from "@fortawesome/free-solid-svg-icons";
import { faTrashAlt,faCheckCircle} from "@fortawesome/free-regular-svg-icons";
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';

library.add(faTrashAlt, faTimesCircle, faEdit, faCheckCircle, faCheck, faArrowUp, faArrowDown );
Vue.component('font-awesome-icon', FontAwesomeIcon);


@Component
export default class Preferences extends Vue {
    mounted() {
        this.getSettings();
        console.log("mounted!!");
    };
        selected = "";
        CategoryTitle = "";
        categories = [];
        Changecategory = [];
        ChangecolorScheme = "";
        selectedCategory = "";
        ChangeCategoryTitle = "";
        responseIfError = "";
       


        getCategoryName(event: string){
            console.log(event);
            console.log(this.categories)
            for (let index = 0; index < this.categories.length; index++){
                if(this.categories[index]['category'] === event)
                {
                    this.Changecategory = this.categories[index];
                }
            }
        }

        getCategoryColor(event: string){
            console.log(event);
            console.log(this.categories)
            for (let index = 0; index < this.categories.length; index++){
                if(this.categories[index]['category'] === event)
                {
                    this.Changecategory = this.categories[index];
                }
            }
        }

        UpdateName() {
            var self = this;
            self.Changecategory['category'] = self.ChangeCategoryTitle;
            var bodyContent = self.Changecategory;
        
            fetch('https://localhost:44366/api/values/UpdateUserSetting/', {
                method: 'POST',
                body: JSON.stringify(bodyContent),
                headers: {'Content-Type': 'application/json'}
                }).then(function() {
                    self.getSettings();
                });
        }

        UpdateColor() {
            var self = this;
            self.Changecategory['colorScheme'] = self.ChangecolorScheme;
            var bodyContent = self.Changecategory;
        
            fetch('https://localhost:44366/api/values/UpdateUserSetting/', {
                method: 'POST',
                body: JSON.stringify(bodyContent),
                headers: {'Content-Type': 'application/json'}
                }).then(function() {
                    self.getSettings();
                });
        }

        getSettings() {
            var self = this;
            fetch('https://localhost:44366/api/values/getSettings/' + self.$store.state.user['Id'])
            .then(function(response) {
                if(response.status !== 200)
                    return self.responseIfError = "empty"
            return response.json();
            })
            .then(function(myJson) {
              self.categories = myJson;
                });
        }

        AddUserSetting() {
            var self = this;
            var bodyContent = {UserId: self.$store.state.user['Id'], 
                               ColorScheme: self.selected, 
                               Category: self.CategoryTitle};
        
            fetch('https://localhost:44366/api/values/AddUserSetting/', {
                method: 'POST',
                body: JSON.stringify(bodyContent),
                headers: {'Content-Type': 'application/json'}
                }).then(function() {
                    self.getSettings();
                });
        }

        emptyMethod() {

        }
        
        presentlist(selected: string){
            console.log(selected);
        };

}
</script>

<style scoped>
.updateCategories{
    border: purple solid 2px;

}
.newCategory{
    border: #376ccd solid 2px;
}
.div1 {
    float: left;
}
.div2{
    margin: 25px;
    word-wrap: break-word;
}
.div2 p {
    margin-left: 25px;
    margin-bottom: 0;
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
.ListItemDiv th a {
    margin-left: 55%;
    margin-right: 2%;
}
.ListItemDiv {
    line-height: 20px;
}
.listItem {
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
.dateString {
    color: #376ccd;
    font-size: 0.8em;
    margin-top: 2px;
    padding-left: 40px;
}
.arrowIcon {
    font-size: 0.8em;
}
.colorscheme1 {
    color: green;
}
.colorscheme2 {
    color: purple;
}
.colorscheme3 {
    color: yellow;
}
.colorscheme4 {
    color: orange;
}
.colorscheme5 {
    color: red;
}
.dueDate {
    color:palegreen;
}
</style>

