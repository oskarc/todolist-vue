<template>
  <div class="Container">
      <h1>Mina Listor</h1> <br>
      <!-- <span>{{JSON.stringify(UserLists, null, 2)}}</span> -->
      <!-- <p>{{List_present}}</p> -->
      <div v-if="responseIfError !== 'empty'">
        <select class="select-css" @change="presentlist($event.target.selectedOptions[0]._value)" name="dropdown" id="dropdown"> 
            <option selected hidden value="empty">Välj Lista</option>
            <option v-for="obj in UserLists" :key="obj['id']" :value="obj">{{obj['title']}}</option>
        </select> 
            <font-awesome-icon @click="createListMode = true" v-show="!createListMode" v-bind:class="{addList: 'addList'}" class="iconPlus" :icon="['fas', 'plus']"></font-awesome-icon> 
            <font-awesome-icon @click="createListMode = false" v-show="createListMode" v-bind:class="{cancelAddList: 'cancelAddList'}" class="iconPlus" :icon="['fas', 'check']"></font-awesome-icon>
        <div v-show="createListMode">
            <p>Skapa Ny Lista</p>
            <input type="text" placeholder="Ange Titel" @keypress.enter="createList($event.target.value)">
        </div>
      </div>
      <div class="ContainerDivParent" v-if="responseIfError !== 'empty' && List_present !== 'empty' && List_present !== undefined">
          <div v-if="List_present['UserId'] !== 0" class="ContainerDiv">
            <div class="listContainer">
                <font-awesome-icon class="editButton" v-bind:class="{'edit': edit }" @click="edit = CompleteListItem(edit)" :icon="['fas', 'edit']" ></font-awesome-icon>
                <ul>
                <div class="ListItemDiv">
                    <font-awesome-icon class="titleIcon" v-show="edit" type="radio" @click="deleteList(List_present['id'])" :icon="['far', 'trash-alt']"></font-awesome-icon>
                    <h1 v-show="!triggerTitleEdit" @click="triggerTitleEdit = keepBool(edit)" :class="['listTitle', {'hoverTitle': edit}]">{{List_present['title']}}</h1>
                    <input v-show="triggerTitleEdit && edit && show('titleInput')" id="titleInput" :value="List_present['title']" ref="titleInput" type="text" @keyup.esc="triggerTitleEdit = false" @blur="triggerTitleEdit = false, EditListTitle(List_present, $event.target.value)" @keypress.enter="triggerTitleEdit = false, EditListTitle(List_present, $event.target.value)">
                        <li class="sortBy" @click="sortByCreatedDate()">Created</li>
                        <li class="sortBy" @click="sortByDueDate()">Due</li>
                        <li class="sortBy" @click="sortByCategory()">Category</li>
                        <li class="sortBy" @click="sortByPriority()">Priority</li>
                        <li>
                            <font-awesome-icon class="arrowIcon" v-show="sort === 2" :icon="['fas', 'arrow-down']"></font-awesome-icon>
                            <font-awesome-icon class="arrowIcon" v-show="sort === 1" :icon="['fas', 'arrow-up']"></font-awesome-icon>
                        </li>
                </div>
                    <li>
                        <div class="ListItemDiv" v-for="item in List_present['listItem']" :key="item['id']">
                            <div class="div1">
                                <font-awesome-icon class="itemIcon" v-show="edit" @click="deleteItem(item)" :icon="['fas', 'times-circle']"></font-awesome-icon>
                                <font-awesome-icon v-show="!edit" v-bind:class="{ 'competedIconDone': item['completed'], 'completedIcon': true}" @click="item['completed'] = CompleteListItem(item['completed']), saveItem(item)" :icon="['fas', 'check']"></font-awesome-icon>
                            </div>
                            <div class="div2">
                                <p      v-show="showDueDate !== item['id']"          @click="showDueDate = allowIfEditMode(item['id'])" :class="['dueDate', SetCategoryClassForItem(item['category']), {'hoverSettings': edit}]">Klart: {{item['dueDate'].replace('T', ' ')}}</p>
                                <input  v-show="edit && showDueDate === item['id'] && show(item['id'], 'itemDueDate')" @blur="EditItemDueDate(item, $event.target.value)"  @keypress.enter="EditItemDueDate(item, $event.target.value)" type="datetime-local" :value="item['dueDate']" :ref="'itemDueDate' + item['id']" name="itemDueDate" :id="'itemDueDate' + item['id']">
                                <span   v-show="showPriority !== item['id']"         @click="showPriority = allowIfEditMode(item['id'])" :class="[item['priority'], {'hoverSettings': edit}]">{{item['priority']}}</span>
                                <select v-show="edit && showPriority === item['id'] && show(item['id'], 'dropdownPriority')" v-model="item['priority']" @blur="showPriority = 0" @change="EditItemPriority(item, $event.target.value)" name="dropdown" :ref="'dropdown' + item['id']" :id="'dropdownPriority' + item['id']">
                                    <option selected hidden value="empty">Prioritet</option>
                                    <option value="Hög">Hög</option>
                                    <option value="Medel">Medel</option>
                                    <option value="Låg">Låg</option>
                                </select>
                                <span v-show="showCategory !== item['id']" @click="showCategory = allowIfEditMode(item['id'])" :class="[SetCategoryClassForItem(item['category']), 'span', {'hoverSettings': edit}]">{{item['category']}}</span>
                                <select v-show="edit && showCategory === item['id'] && show(item['id'], 'dropDownCategory')" @blur="showCategory = 0" class="editCategory" @change="EditCategoryForItem(item, $event.target.value)" :ref="'dropDownCategory' + item['id']" name="dropdown" :id="'dropDownCategory' + item['id']">
                                    <option selected hidden value="empty">Kategorier</option>
                                    <option :class="obj['colorScheme']" v-for="obj in categories" :key="obj['id']" :value="obj['categoryId']">{{obj['category']}}</option>
                                </select>
                                <p v-show="edit && showItem !== item['id']" class="listItem" @click="triggerItemTextEdit = keepBool(edit), hide = changeBool(hide), showItem = item['id']" v-bind:class="[{ 'completed': item['completed']}, SetCategoryClassForItem(item['category']), 'listItemText', {'hoverSettings': edit}]">{{item['listItemText']}}</p>
                                <p v-show="!edit" class="listItem" @click="item['completed'] = CompleteListItem(item['completed']), saveItem(item)" v-bind:class="[{ 'completed': item['completed']}, SetCategoryClassForItem(item['category']), 'listItemText']">{{item['listItemText']}}</p>
                                <input :value="item['listItemText']" v-show="edit && showItem === item['id'] && show(item['id'], 'input')" type="text" :ref="'input' + item['id']" class="editItem" name="editItem" :id="'input' + item['id']" @blur="EditItemText(item, $event.target.value), triggerItemTextEdit = changeBool(triggerItemTextEdit), hide = changeBool(hide)" @keyup.esc="showItem = 0, hide = changeBool(hide)" @keypress.enter="EditItemText(item, $event.target.value), triggerItemTextEdit = changeBool(triggerItemTextEdit), hide = changeBool(hide)"/>
                                <p class="dateString">Skapad: {{item['created'].replace('T', ' ')}}</p>
                            </div>
                        </div>
                        <div v-show="edit">
                            <input @keypress.enter="AddItem($event.target.value), update = 1"  v-model="newItem" class="addItem" placeholder="Skriv syssla eller sak"/>
                            <!-- <select class="AddItemCategory" @change="category = $event.target.value" name="dropdown" id="dropdown">
                                <option selected hidden value="empty">Kategorier</option>
                                <option v-for="obj in categories" :key="obj['id']" :value="obj['category']">{{obj['category']}}</option>
                            </select>
                            <select class="" @change="Priority = $event.target.value" name="dropdown" id="dropdown">
                                <option selected hidden value="empty">Prioritet</option>
                                <option value="Hög">Hög</option>
                                <option value="Medel">Medel</option>
                                <option value="Låg">Låg</option>
                            </select>
                            <input v-model="dueDate" type="datetime-local" name="dueDate" id="dueDate"> -->
                        </div>
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
import {List1, ListItem1, UserSettings} from '@/models/types';
import { library, config } from '@fortawesome/fontawesome-svg-core';
import { faTimesCircle, faEdit, faCheck, faArrowUp, faArrowDown, faPlus, faMehRollingEyes} from "@fortawesome/free-solid-svg-icons";
import { faTrashAlt,faCheckCircle} from "@fortawesome/free-regular-svg-icons";
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';

library.add(faTrashAlt, faTimesCircle, faEdit, faCheckCircle, faCheck, faArrowUp, faArrowDown, faPlus );
Vue.component('font-awesome-icon', FontAwesomeIcon);


@Component
export default class Api extends Vue {
    mounted() {
        this.getUserLists(false);
        this.getSettings();
        console.log("mounted!!");
    };
        newItem = "";
        showDueDate = 0;
        showPriority = 0;
        showItem = 0;
        showCategory = 0;
        hide = false;
        title = "";
        triggerTitleEdit = false;
        triggerItemTextEdit = false;
        firstrun = 1;
        update = 0;
        edit = false;
        createListMode = false;
        dropdown = "Välj Lista";
        items= "null";
        inputValue= "this is input";
        inputList= 'test';
        input1= "";
        input2= "";
        userid= 0;
        listid= 0;
        UserLists = new Array<List1>();
        responseIfError = "";
        debug= false;
        List_present = new List1;
        debug2 = "";
        selectedlist = "";
        event = "";
        sort = 1;
        categories = [];
        dueDate = "";
        category = "";
        categoryClass = "";
        Priority = "";
        addList = true;
        cancelAddList = true;
        newList = new List1;
        addedList = new List1;


        emptyMethod() {

        }
        allowIfEditMode(value: number) {
            if(this.edit)
                return value;
            else
                return 0;
        }

        show(ref: string, type = "") {
            var self = this;
            setTimeout(function () {
                if(type !=="")
                        (<HTMLInputElement>document.getElementById(type+ref)).focus();
                    else
                        (<HTMLInputElement>self.$refs[type+ref]).focus();
                    }, 1)
                return true;
        }

        changeBool(bool: boolean) {
            if(bool)
                return false;
            if(!bool)
                return true;
        }
        keepBool(bool: boolean) {
            if(bool)
                return true;
        }
        createList(titel: string) {
            this.newList.UserId = this.$store.state.user['Id']
            this.newList.Title = titel;
            this.AddList(this.newList);
            console.log(this.List_present)
            this.newList = new List1;
            console.log(this.List_present)
            this.createListMode = false;
        }

        SetCategoryClassForItem(itemCategory: string) {
            let value = "";
            for (let index = 0; index < this.categories.length; index++){
                if(itemCategory === this.categories[index]['category'])
                    value = this.categories[index]['colorScheme'];
            }
            return value;
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

        sortByCreatedDate() {
            console.log(this.sort)
            if(this.sort === 1){
                this.List_present.listItem.sort(function(a, b) {
                    if (Date.parse(a.created) > Date.parse(b.created)) 
                        return 1;
                    if (Date.parse(a.created) < Date.parse(b.created)) 
                        return -1;
    
                    return 0;
                });
            }

            if(this.sort === 2) {
                this.List_present.listItem.sort(function(a, b) {
                    if ( Date.parse(a.created) > Date.parse(b.created)) 
                        return -1;
                    if (Date.parse(a.created) < Date.parse(b.created)) 
                        return 1;
                    
                    return 0;
                });
            }
            if(this.sort === 2)
                this.sort = 1;
            else
                this.sort = 2;
        };

        sortByDueDate() {
            console.log(this.sort)
            if(this.sort === 1){
                this.List_present.listItem.sort(function(a, b) {
                    if (Date.parse(a.dueDate) > Date.parse(b.dueDate)) 
                        return 1;
                    if (Date.parse(a.dueDate) < Date.parse(b.dueDate)) 
                        return -1;
    
                    return 0;
                });
            }

            if(this.sort === 2) {
                this.List_present.listItem.sort(function(a, b) {
                    if ( Date.parse(a.dueDate) > Date.parse(b.dueDate)) 
                        return -1;
                    if (Date.parse(a.dueDate) < Date.parse(b.dueDate)) 
                        return 1;
                    
                    return 0;
                });
            }
            if(this.sort === 2)
                this.sort = 1;
            else
                this.sort = 2;
        };


        sortByCategory() {
            if(this.sort === 1){
                this.List_present.listItem.sort(function(a, b) {
                    console.log(a.category, b.category)
                    if (a.category > b.category) 
                        return 1;
                    if (a.category < b.category) 
                        return -1;
    
                    return 0;
                });
            }

            if(this.sort === 2) {
                this.List_present.listItem.sort(function(a, b) {
                    console.log(a.category, b.category)
                    if ( a.category > b.category) 
                        return -1;
                    if (a.category < b.category) 
                        return 1;
                    
                    return 0;
                });
            }
            if(this.sort === 2)
                this.sort = 1;
            else
                this.sort = 2;
        };
        
        sortByPriority() {
            if(this.sort === 1){
                this.List_present.listItem.sort(function(a, b) {
                    console.log(a.priority,b.priority)
                    if(a.priority === null || b.priority === null)
                        return -2;
                    if (a.priority === "Hög" && b.priority === "Medel" || b.priority === "Låg") 
                        return 1;
                    if (a.priority === 'Låg' && b.priority === "Medel" || b.priority === "Hög") 
                        return -1;

                    return 0;
                });
            }

            if(this.sort === 2) {
                this.List_present.listItem.sort(function(a, b) {
                    console.log(a.priority,b.priority)
                    if(a.priority === null || b.priority === null)
                        return -2;
                    if (a.priority === "Hög" && b.priority === "Medel" || b.priority === "Låg") 
                        return -1;
                    if (a.priority === 'Låg' && b.priority === "Medel" || b.priority === "Hög") 
                        return 1;

                    return 0;
                });
            }
            if(this.sort === 2)
                this.sort = 1;
            else
                this.sort = 2;
        };

        presentlist(event: any){
            this.userid = event.userId;
            this.listid = event.listId;
            this.selectedlist = event;
            console.log(event);
            this.List_present = event;
        };

        getUserLists(showLast: boolean) {
          var self = this;
          console.log(self.$store.state.user)
          fetch('https://localhost:44366/api/values/UserLists/' + self.$store.state.user['Id'])
            .then(function(response) {
                if(response.status !== 200)
                    return self.responseIfError = "empty"
            return response.json();
            })
            .then(function(myJson) {
                console.log("myjson" + JSON.stringify(myJson, null, 2))
                var newList: any;
                if(showLast)
                    self.List_present = myJson[myJson.length -1];
                // @ts-ignore
                myJson.forEach(element => {
                    if(element.id === self.List_present.id)
                        newList = element;
                    console.log(element);
                    })
                        console.log("present" + JSON.stringify(self.List_present, null, 2))
                        console.log("after" + newList)
                        self.items = JSON.stringify(myJson, null, 2);
                        self.UserLists = myJson;
                        self.$store.state.z_string = myJson;
                        if(self.List_present !== undefined)
                            self.List_present.listItem.sort()

                        })
        };

        getUserListsOnDelete() {
          var self = this;
          console.log(self.$store.state.user)
          fetch('https://localhost:44366/api/values/UserLists/' + self.$store.state.user['Id'])
            .then(function(response) {
                if(response.status !== 200)
                    return self.responseIfError = "empty"
            return response.json();
            })
            .then(function(myJson) {
                console.log("myjson" + JSON.stringify(myJson, null, 2))
                var newList: any;
                // @ts-ignore
                myJson.forEach(element => {
                    if(element.id === self.List_present.id)
                        newList = element;
                    console.log(element);
                    })
                        console.log("present" + JSON.stringify(self.List_present, null, 2))
                        console.log("after" + newList)
                        self.items = JSON.stringify(myJson, null, 2);
                        self.UserLists = myJson;
                        self.$store.state.z_string = myJson;
                        self.List_present = newList;
                        if(self.List_present !== undefined)
                            self.List_present.listItem.sort()

                        })
        };

        getUserList(listid: number) {
            console.log(this.List_present)
            var self = this;
            console.log(self.$store.state.user)
            var bodyContent = { UserId: self.$store.state.user['Id'], ListId: listid};
            fetch('https://localhost:44366/api/values/UserList/', {
              method: 'POST',
              body: JSON.stringify(bodyContent),
              headers: {'Content-Type': 'application/json'}}).then(function(response) {
                if(response.status !== 200)
                    return self.responseIfError = "empty"
                return response.json();
            })
            .then(function(myJson) {
                    self.List_present = myJson;
                    self.List_present.listItem.sort()

                    for (let index = 0; index < self.UserLists.length; index++){
                    if(listid === self.UserLists[index]['listId']){
                        self.UserLists[index] = self.List_present;
                        }
                    }
                    })
         };

        getItemById(itemid: number) {
            var self = this;
            fetch('https://localhost:44366/api/values/getItemById/' + itemid, {
              method: 'GET',
              headers: {'Content-Type': 'application/json'}})
              .then(function(response) {
                if(response.status !== 200)
                    return self.responseIfError = "empty"
                return response.json();
            })
            .then(function(myJson) {
                console.log("here" + myJson)

                    for (let index = 0; index < self.List_present.listItem.length; index++){
                        if(itemid === self.List_present.listItem[index]['id']){
                            self.List_present.listItem[index] = myJson;
                        }
                    }

                    self.List_present.listItem.sort()
                    })
         };

        saveAttempt() {
            var self = this;
            console.log(self.inputList);
            console.log(self.UserLists)
          fetch('https://localhost:44366/api/values/save/', {
              method: 'POST',
              body: JSON.stringify(self.UserLists),
              headers: {'Content-Type': 'application/json'}});
          };

        saveItem(listitem: ListItem1) {
            var self = this;
                fetch('https://localhost:44366/api/values/SaveItem/', {
                    method: 'POST',
                    body: JSON.stringify(listitem),
                    headers: {'Content-Type': 'application/json'}}).then(function() {
                        self.getItemById(listitem.id)
                    });
          };

        EditItemText(listitem: ListItem1, newText: string){
                var self = this;
                console.log(newText);
                listitem.ListItemText = newText;
                fetch('https://localhost:44366/api/values/SaveItem/', {
                            method: 'POST',
                            body: JSON.stringify(listitem),
                            headers: {'Content-Type': 'application/json'}}).then(function() {
                                self.getItemById(listitem.id);
                            });
                self.showItem = 0;
            }

        EditItemPriority(listitem: ListItem1, Priority: string){
                var self = this;
                console.log(Priority);
                listitem.priority = Priority;
                fetch('https://localhost:44366/api/values/SaveItem/', {
                            method: 'POST',
                            body: JSON.stringify(listitem),
                            headers: {'Content-Type': 'application/json'}}).then(function() {
                                // self.get1();
                                self.getItemById(listitem.id);
                            });

                self.showPriority = 0;
            }

        EditItemDueDate(listitem: ListItem1, DueDate: string){
                var self = this;
                console.log(DueDate);
                listitem.dueDate = DueDate;
                fetch('https://localhost:44366/api/values/SaveItem/', {
                            method: 'POST',
                            body: JSON.stringify(listitem),
                            headers: {'Content-Type': 'application/json'}}).then(function() {
                                // self.get1();
                                self.getItemById(listitem.id);
                            });

                self.showDueDate = 0;
            }
        EditListTitle(list: List1, title: string){
                var self = this;
                console.log(title);
                list.Title = title;
                fetch('https://localhost:44366/api/values/UpdateTitle/', {
                            method: 'POST',
                            body: JSON.stringify(list),
                            headers: {'Content-Type': 'application/json'}}).then(function() {
                                self.getUserList(list.listId);
                            });
            }
       
        EditCategoryForItem(listitem: ListItem1, newCategoryId: string){
                var categoryId = parseInt(newCategoryId, 10);
                for (let index = 0; index < this.categories.length; index++){
                    if((categoryId as number) === this.categories[index]['categoryId']){
                        listitem.category = this.categories[index]['category'];
                    }
                }
                  var self = this;
                  listitem.categoryId = categoryId;
                  fetch('https://localhost:44366/api/values/SaveItem/', {
                  method: 'POST',
                  body: JSON.stringify(listitem),
                  headers: {'Content-Type': 'application/json'}}).then(function(){
                    self.getItemById(listitem.id);
                  });
    
                  self.showCategory = 0;
              }

        AddList(list: List1) {
            var self = this;
            console.log(list);
          fetch('https://localhost:44366/api/values/', {
              method: 'POST',
              body: JSON.stringify(list),
              headers: {'Content-Type': 'application/json'}
                }).then(function(response) {
                  if(response.status === 200){
                     // self.resetForm();
                  }
                  else {
                      alert("Något gick fel! Testa igen.");
                  }
                  }).then(function(){
                     self.getUserLists(true);
                  })
          };

        AddItem(ListItemText: string) {
                var self = this;
                self.event = ListItemText;
                var bodyContent = {ListId: self.List_present.listId, UserId: self.$store.state.user['Id'], ListItemText: ListItemText, dueDate: self.dueDate, category: self.category, Priority: self.Priority};
                console.log(bodyContent)
                fetch('https://localhost:44366/api/values/AddItem/', {
                    method: 'POST',
                    body: JSON.stringify(bodyContent),
                    headers: {'Content-Type': 'application/json'}
                    }).then(function(){   
                        self.firstrun = 0;
                         self.getUserList(self.List_present.listId);
                         self.newItem = "";
                    });
          };


        postAttempt2() {

                var self = this;
                var bodyContent = {ListId: self.listid, UserId: self.userid};
                console.log(self.inputList);

            fetch('https://localhost:44366/api/values/UserList', {
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
                    });
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
                });
            };

        deleteList(id: number) {
            var result = window.confirm("Vill du ta bort listan permanent?");
            if(result){
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
                    }).then(function(myJson) {
                        self.List_present = new List1;
                        self.getUserLists(false);
                });
            }
            };

        deleteItem(listItem: ListItem1) {
            var self = this;
            fetch('https://localhost:44366/api/values/DeleteItem/' + listItem.id, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json'
                },
            }).then(function(response) {
                if(response.status !== 200)
                     alert("error")
                else{
                    self.getUserList(listItem.listId);
                }
            });
        };

}
</script>

<style scoped>
.sortBy{

}
.sortBy:hover {
    font-size: 1.4em;
    color: grey;
}
.edit {
    color:#cc3300;
}
#titleInput {
   position: relative;
   bottom: 6px;
}
.ContainerDivParent {
    text-align: center;
}
.Container {
    text-align: center;
}
.span {
    margin-left: 10px;
}
.listTitle{
    display: inline;
}
.hoverTitle:hover{
    font-size: 2.05em;
    color: grey;
}
.hoverSettings:hover{
font-size: 1.02em;
    color: grey;
}
.iconPlus{
    font-size: 1.5em;
    display: inline;
    margin-left: 20px;
}
.addList{
    color: teal;
}
.cancelAddList {
    color: tomato;
}
.Låg {
    text-decoration: underline;
    color: #339900;
}
.Medel {
    text-decoration: underline;
    color: #FFAC0E
}
.Hög {
    text-decoration: underline;
    color: #cc3300;
}
.editItem{
    width: 95%;
    margin-left: 25px;
}
.editCategory{
    margin-left: 25px;
}
.div1 {
    float: left;
    margin-right:1px;
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
    /* max-width: 800px; */
    text-align: left;
}
.listContainer ul {
    min-width: 150px;
    justify-content: space-around;
}
.listContainer li {
    padding: 4px;
    padding-left: 10%;
    list-style-type: none;
    max-width: 70%;
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
.ListItemDiv li {
    font-weight: bolder;
    padding: 0;
    font-size: 1.3em;
    margin-left: 70%;
}
.completed{
    text-decoration: line-through;
    text-decoration-color: black;
}
.ContainerDiv{
    border: 2px solid #3bb273;
    margin: 40px;
    min-width: 500px;
    /* max-width: 80%; */
}
/* TODO check this, options is too long */
.select-css {
    display: inline;
    font-size: 16px;
    font-family: sans-serif;
    font-weight: 700;
    color: #444;
    line-height: 1.3;
    padding: .6em 1.4em .5em .8em;
    max-width: 30%; 
    box-sizing: border-box;
    margin-left: 8%;
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
/* TODO check this, options is too long */
.select-css option {
    font-weight: normal;
    overflow: hidden;
}


body {
  padding: 3rem;
}
.titleIcon{
    font-size: 18px;
    padding-right: 8px;
    padding-bottom: 5px;
}
.itemIcon {
    /* font-size: 1.6em; */
    height: 27px;
    width: 27px;
    padding-right: 22px;
    position: relative;
    top: 56px;
}
.itemIcon:hover {
    height: 28px;
    width: 28px;
    color:red;
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
    font-size: 1.05em;
    /* color: #3bb273; */
    
}
.completedIcon {
    border: #3bb273 solid 1px;
    border-radius: 50%;
    font-size: 1.0em;
    margin-right:21px;
    padding: 5px;
    position: relative;
    top: 56px;
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
    margin-left: 50px;
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
.colorscheme1{
    color: #376ccd;
}
.colorscheme1.listItemText {
    border: #376ccd 2px solid;
}
.colorscheme2 {
    color: #7768ae;
}
.colorscheme2.listItemText {
    border: #7768ae 2px solid;
}
.colorscheme3 {
    color: #3bb273;
}
.colorscheme3.listItemText {
    border: #3bb273 2px solid;
}
.colorscheme4 {
    color: #e1bc29;

}
.colorscheme4.listItemText {
    border: #e1bc29 2px solid;
}
.colorscheme5 {
    color: #e15554;
}
.colorscheme5.listItemText {
    border: #e15554 2px solid;
}
.dueDate {
}
.AddItemCategory{
    margin-left: 50px;
}
</style>

