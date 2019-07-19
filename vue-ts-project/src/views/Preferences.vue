<template>
  <div style="text-align: center;" class="">
      <h1>Mina Inställningar</h1> <br>
  <div style="max-width: 50%; margin-left: 25%;" class="divTable blueTable">
        <div class="divTableHeading">
            <div class="divTableRow">
                <div class="divTableHead">
                    <p>Kategori</p>
                </div>
                <div class="divTableHead">
                   <p>Färgschema</p>
                </div>
                <div class="divTableHead" style="width:40px; ">
                    <!-- <font-awesome-icon @click="toggleDelete = changeBool(toggleDelete)"  class="iconPlus cancelAddCategory" :icon="['fas', 'check']"></font-awesome-icon> -->
                </div>
            </div>
        </div>
        <div class="divTableBody">
            <div class="divTableRow" :key="item['Id']" v-for="item in categories">
                <div  class="divTableCell vForCells updateCategory">
                    <!-- <p :class="item['colorScheme']" :id="item['id']" v-show="editTitle !== item['id']" @click="editTitle = item['id']" >{{item['category']}}</p> -->
                    <input :class="item['colorScheme']" @blur="changeCategory = item, ChangeCategoryTitle = $event.target.value, UpdateName()" :value="item['category']" @keyup.enter="changeCategory = item, ChangeCategoryTitle = $event.target.value, UpdateName()" @keyup.esc="editTitle = false" type="text" :ref="'input' + item['id']" :id="'input' + item['id']">
                </div>
                <div class="divTableCell vForCells"  >
                    <!-- <p v-show="edit !== item['id']" @click="edit = item['id'], expand(item['id'])" :ref="item['id']" :id="item['id']" :class="item['colorScheme']" >{{item['colorScheme']}}</p> -->
                    <select :class="item['colorScheme']" v-model="item['colorScheme']"  @change="changeCategory = item, ChangecolorScheme = $event.target.value, UpdateColor()" name="dropdown" :ref="'select' + item['id']" :id="'select' + item['id']">
                        <option class="colorscheme1" value="colorscheme1">ColorScheme 1</option>
                        <option class="colorscheme2" value="colorscheme2">ColorScheme 2</option>
                        <option class="colorscheme3" value="colorscheme3">ColorScheme 3</option>
                        <option class="colorscheme4" value="colorscheme4">ColorScheme 4</option>
                        <option class="colorscheme5" value="colorscheme5">ColorScheme 5</option>
                    </select>
                </div>
                <div  class="divTableCell deleteIcon" v-show="!toggleDelete">
                   <font-awesome-icon @click="DeleteUserSetting(item)" class="iconPlus" :icon="['far', 'trash-alt']"></font-awesome-icon>
                </div>
            </div>
            <div class="divTableRow">
                <!-- <div class="divTableCell20">
                    <font-awesome-icon @click="newCategoryToggle = true" v-show="!newCategoryToggle"  class="iconPlus addCategory" :icon="['fas', 'plus']"></font-awesome-icon> 
                    <font-awesome-icon @click="newCategoryToggle = false" v-show="newCategoryToggle"  class="iconPlus cancelAddCategory" :icon="['fas', 'check']"></font-awesome-icon>
                </div> -->
                <div class="divTableCell80 addCategoryCell">
                    <font-awesome-icon @click="newCategoryToggle = true" v-show="!newCategoryToggle"  class="iconPlus addCategory" :icon="['fas', 'plus']"></font-awesome-icon> 
                    <font-awesome-icon @click="newCategoryToggle = false" v-show="newCategoryToggle"  class="iconPlus cancelAddCategory" :icon="['fas', 'check']"></font-awesome-icon>
                    <input v-show="newCategoryToggle && showInput('newCategory')" ref="newCategory" type="text" @blur="newCategoryToggle = false" v-model="CategoryTitle" @keyup.enter="AddUserSetting($event.target.value)"/>
                </div>
                <div class="divTableCell">
                </div>
                <div class="divTableCell">
                </div>
            </div>
        </div>
    </div>
 </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import state from 'vuex';
import {List1, ListItem1, UserSetting} from '@/models/types';
import { library, config } from '@fortawesome/fontawesome-svg-core';
import { faTimesCircle, faEdit, faCheck, faArrowUp, faArrowDown, faPlus} from "@fortawesome/free-solid-svg-icons";
import { faTrashAlt,faCheckCircle} from "@fortawesome/free-regular-svg-icons";
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import vSelect from 'vue-select';

library.add(faTrashAlt, faTimesCircle, faEdit, faCheckCircle, faCheck, faArrowUp, faArrowDown, faPlus );
Vue.component('font-awesome-icon', FontAwesomeIcon);
Vue.component('v-select', vSelect)

@Component
export default class Preferences extends Vue {
    mounted() {
        this.getSettings();
        console.log("mounted!!");
    };
        toggleDelete = false;
        newCategoryToggle = false;
        editTitle = 0;
        edit = 0;
        selected = "";
        CategoryTitle = "";
        categories = [];
        changeCategory = new UserSetting;
        ChangecolorScheme = "";
        selectedCategory = "";
        ChangeCategoryTitle = "";
        responseIfError = "";
        options = ['colorscheme1', 
                   'colorscheme2', 
                   'colorscheme3', 
                   'colorscheme4', 
                   'colorscheme5' ]
       
        showInput(ref: string, type = "") {
            var self = this;
            setTimeout(function () {
                if(type !==""){
                    console.log("id");
                    (<HTMLElement>document.getElementById(type+ref)).focus();
                }
                    else
                        (<HTMLElement>self.$refs[type+ref]).focus();
                    }, 1)
                return true;
        }

        showSelect(ref: string, type = "") {

            console.log(type)
            console.log(document.getElementById(type+ref))
            console.log(this.$refs[type+ref])
            var self = this;
            setTimeout(function () {
                if(type !==""){
                    console.log("hit")
                        console.log(self.$refs)
                        console.log(self.$refs[type+ref])
                        console.log(self.$refs[type+ref][0])
                        console.log(self.$refs[type+ref][0].$el)
                        self.$refs[type+ref][0].open = true;
                        self.$refs[type+ref][0].focus = true;
                        // self.$refs[type+ref][0].disabled = true;
                }
                    else{
                        (<HTMLInputElement>self.$refs[type+ref]).focus();
                        (<HTMLInputElement>document.getElementById(type+ref)).click();
                    }
                    }, 1)
                return true;
        }


       log(value: string) {
           console.log(value);
       }
        keepBool(bool: boolean) {
            if(bool)
                return true;
            else 
                return false;
        }

        expand(itemId) {
            //doesnt work yet
            // var id = 'select' + itemId;
            // console.log(this.$refs[id])
            // var self = this;
            // setTimeout(function () {
            //     self.$refs[id][0].click();
            //         }, 1)
        }

        changeBool(bool: boolean) {
            console.log(bool)
            if(!bool)
                return true;
            if(bool)
                return false;
        }

        getCategoryName(event: string){
            console.log(event);
            console.log(this.categories)
            for (let index = 0; index < this.categories.length; index++){
                if(this.categories[index]['category'] === event)
                {
                    this.changeCategory = this.categories[index];
                }
            }
        }

        getCategoryColor(event: string){
            console.log(event);
            console.log(this.categories)
            for (let index = 0; index < this.categories.length; index++){
                if(this.categories[index]['category'] === event)
                {
                    this.changeCategory = this.categories[index];
                }
            }
        }

        UpdateName() {
            var self = this;
            console.log("Hit!")
            console.log(self.ChangeCategoryTitle)
            console.log(self.changeCategory)
            console.log("after hit!")
            self.changeCategory.Category = self.ChangeCategoryTitle;
            var bodyContent = self.changeCategory;
        
            fetch('https://localhost:44366/api/values/UpdateUserSetting/', {
                method: 'POST',
                body: JSON.stringify(bodyContent),
                headers: {'Content-Type': 'application/json'}
                }).then(function() {
                    self.getSettings();
                });

                self.editTitle = 0;
        }

        UpdateColor() {
            var self = this;
            self.changeCategory.ColorScheme = self.ChangecolorScheme;
            var bodyContent = self.changeCategory;
        
            fetch('https://localhost:44366/api/values/UpdateUserSetting/', {
                method: 'POST',
                body: JSON.stringify(bodyContent),
                headers: {'Content-Type': 'application/json'}
                }).then(function() {
                    self.getSettings();
                });

                self.edit = 0;
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

        AddUserSetting(categoryTitle: string) {
            var self = this;
            var bodyContent = {UserId: self.$store.state.user['Id'], 
                               ColorScheme: "colorscheme1", 
                               Category: categoryTitle};
        
            fetch('https://localhost:44366/api/values/AddUserSetting/', {
                method: 'POST',
                body: JSON.stringify(bodyContent),
                headers: {'Content-Type': 'application/json'}
                }).then(function() {
                    self.getSettings();
                });
                self.newCategoryToggle = false;
        }
        DeleteUserSetting(item: UserSetting) {
            console.log(item)
            var result = window.confirm("Vill du ta bort kategorin permanent?");
            if(result){
                var self = this;
                item.UserId = self.$store.state.user['Id'];
                var bodyContent = item;
            
                fetch('https://localhost:44366/api/values/DeleteUserSetting/', {
                    method: 'DELETE',
                    body: JSON.stringify(bodyContent),
                    headers: {'Content-Type': 'application/json'}
                    }).then(function() {
                        self.getSettings();
                    });
            }
        }

        emptyMethod() {

        }
        
        presentlist(selected: string){
            console.log(selected);
        };

}
</script>

<style scoped>
/* svg {
    display: flex;
    float: right;
} */
.container {
    max-width: 40%;
    margin-left: 30%;
}
table {
    /* background-color: green; */
}

th {
    border: #3bb273 1px solid;
    background-color: white;
}

td {
    background-color: white;
    font-size: 2em;
    min-width: 50%;
}

/* .iconPlus{
    font-size: 1.5em;
    display: inline;
    margin-right: 20px;
} */
.addCategory{
    color: teal;
}
.cancelAddCategory {
    color: tomato;
    font-size: 0.8em;
}
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
/* .div2 p {
    margin-left: 25px;
    margin-bottom: 0;
} */

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
/* 
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
} */

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
    color: #376ccd;
    border: #376ccd 2px solid;
}
.colorscheme2 {
    color: #7768ae;
    border: #7768ae 2px solid;
}
.colorscheme3 {
    color: #3bb273;
    border: #3bb273 2px solid;
}
.colorscheme4 {
    color: #e1bc29;
    border: #e1bc29 2px solid;

}
.colorscheme5 {
    color: #e15554;
    border: #e15554 2px solid;

}
.dueDate {
    color:palegreen;
}

p {
    font-size: 2em;
    margin: 4px;
    padding-left: 6px;
    text-align: center;
}
.divTable {
    max-width: 200px;
    margin-right: 200px;
}
div.blueTable {
  border: 1px solid #376ccd;
  background-color: white;
  width: 100%;
  text-align: left;
  /* border-collapse: collapse; */
}
.divTable.blueTable .divTableCell, .divTable.blueTable .divTableHead {
  /* border: 1px solid #AAAAAA; */
  padding: 3px 2px;
}
.divTable.blueTable .divTableBody .divTableCell {
  font-size: 13px;
}
.divTable.blueTable .divTableRow:nth-child(even) {
  background: rgb(214, 235, 255);
}
.divTable.blueTable .divTableHeading {
    /* text-align: center; */
  background: #5080da;
  /* background: -moz-linear-gradient(top, #5592bb 0%, #327cad 66%, #1C6EA4 100%);
  background: -webkit-linear-gradient(top, #5592bb 0%, #327cad 66%, #1C6EA4 100%);
  background: linear-gradient(to bottom, #5592bb 0%, #327cad 66%, #1C6EA4 100%); */
  /* border-bottom: 2px solid #444444; */
}
.divTable.blueTable .divTableHeading .divTableHead {
  font-size: 15px;
  font-weight: bold;
  color: #FFFFFF;
  /* border-left: 2px solid #D0E4F5; */
}
.divTable.blueTable .divTableHeading .divTableHead:first-child {
  border-left: none;
  border-top: none;
}

.blueTable .tableFootStyle {
  font-size: 14px;
  font-weight: bold;
  color: #FFFFFF;
  background: #D0E4F5;
  background: -moz-linear-gradient(top, #dcebf7 0%, #d4e6f6 66%, #D0E4F5 100%);
  background: -webkit-linear-gradient(top, #dcebf7 0%, #d4e6f6 66%, #D0E4F5 100%);
  background: linear-gradient(to bottom, #dcebf7 0%, #d4e6f6 66%, #D0E4F5 100%);
  /* border-top: 2px solid #444444; */
}
.blueTable .tableFootStyle {
  font-size: 14px;
}
.blueTable .tableFootStyle .links {
	 /* text-align: right; */
}
.blueTable .tableFootStyle .links a{
  display: inline-block;
  background: royalblue;
  color: #FFFFFF;
  /* padding: 2px 8px; */
  border-radius: 5px;
}
.blueTable.outerTableFooter {
  border-top: none;
}
.blueTable.outerTableFooter .tableFootStyle {
  /* padding: 3px 5px;  */
}
/* DivTable.com */
.divTable{ display: table; }
.divTableRow { display: table-row; }
.divTableHeading { display: table-header-group;}
.divTableCell, .divTableHead { display: table-cell;}
.divTableHeading { display: table-header-group;}
.divTableFoot { display: table-footer-group;}
.divTableBody { display: table-row-group;}

.vForCells {
    text-align: center;
}
.addCategoryCell{
    height: 41px; 
}
.addCategoryCell svg {
    /* margin-left: 20px; */
    padding-left: 6%;
    font-size: 1.2em;
    padding-right: 23px;
    position: relative;
    top: 10px;
}
.addCategoryCell input {
    font-size: 1.2em;
    max-width: 60%;
    padding-left: 1%;
    margin-left: 0%;
    position: relative;
    top: 6px;
}
.updateCategory {
    text-align: center;
}
.updateCategory input {
    text-align: center;
    font-size: 1.8em;
    max-width: 60%;
    padding-left: 4px;
    margin-left: 4px;

}
/* .updateCategory p {
    text-align: center;
} */
.divTableHead svg {
    text-align: center;
}
.divTableCell20 {
    /* min-width: 20%;
    max-width: 20%; */
    display: table-cell;
}
.divTableCell80{
    /* min-width: 80%;
    max-width: 80%; */
    /* display: table-cell; */
}
.deleteIcon{
    text-align: center;
    
}
.deleteIcon svg {
    position: relative;
    bottom: 3px;
    font-size: 1.2em;
    margin-right: 10px;
}

.deleteIcon svg:hover {
    font-size: 1.25em;
    color: red;
}
select {
    font-size: 2em;
    padding-left: 10px;
    padding-right: 10px;
    /* for Firefox */
    -moz-appearance: none;
    /* for Chrome */
    -webkit-appearance: none;
}
</style>

