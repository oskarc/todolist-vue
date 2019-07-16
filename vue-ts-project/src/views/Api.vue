<template>
  <div class="">
      <h1>Mina Listor</h1> <br>
      <!-- <p>{{List_present}}</p> -->
      <div v-if="responseIfError !== 'empty'">
        <select class="select-css" @change="presentlist($event.target.selectedOptions[0]._value)" name="dropdown" id="dropdown"> 
            <option selected hidden value="empty">Välj Lista</option>
            <option v-for="obj in responseContainerList1" :key="obj['id']" :value="obj">{{obj['title']}}</option>
        </select> 
            <font-awesome-icon @click="createListMode = true" v-show="!createListMode" v-bind:class="{addList: 'addList'}" class="iconPlus" :icon="['fas', 'plus']"></font-awesome-icon> 
            <font-awesome-icon @click="createListMode = false" v-show="createListMode" v-bind:class="{cancelAddList: 'cancelAddList'}" class="iconPlus" :icon="['fas', 'check']"></font-awesome-icon>
        <div v-show="createListMode">
            <p>Create</p>
            <input type="text" placeholder="Ange Titel" @keypress.enter="createList($event.target.value)">
        </div>
      </div>
      <div v-if="responseIfError !== 'empty' && List_present !== 'empty' && List_present !== undefined">
          <div v-if="List_present['UserId'] !== 0" class="ContainerDiv">
            <div class="listContainer">
                <font-awesome-icon class="editButton" @click="edit = CompleteListItem(edit)" :icon="['fas', 'edit']" ></font-awesome-icon>
                <ul>
                <div class="ListItemDiv">
                    <font-awesome-icon class="titleIcon" v-show="edit" type="radio" @click="delete1(List_present['id'])" :icon="['far', 'trash-alt']"></font-awesome-icon>
                    <h1 v-show="!triggerTitleEdit" @click="triggerTitleEdit = keepBool(edit)" class="listTitle">{{List_present['title']}}</h1>
                        <input v-show="triggerTitleEdit && edit" :value="List_present['title']" type="text" @keyup.esc="triggerTitleEdit = false" @keypress.enter="triggerTitleEdit = false, EditListTitle(List_present, $event.target.value)">
                        <li @click="sortByCreatedDate()">Created</li>
                        <li @click="sortByDueDate()">Due</li>
                        <li @click="sortByCategory()">Category</li>
                        <li @click="sortByPriority()">Priority</li>
                        <li>
                            <font-awesome-icon class="arrowIcon" v-show="sort === 2" :icon="['fas', 'arrow-down']"></font-awesome-icon>
                            <font-awesome-icon class="arrowIcon" v-show="sort === 1" :icon="['fas', 'arrow-up']"></font-awesome-icon>
                        </li>
                </div>
                    <li>
                        <div class="ListItemDiv" v-for="item in List_present['listItem']" :key="item['id']">
                            <div class="div1">
                                <font-awesome-icon class="itemIcon" v-show="edit" @click="deleteItem(item['id'])" :icon="['fas', 'times-circle']"></font-awesome-icon>
                                <font-awesome-icon v-bind:class="{ 'competedIconDone': item['completed'], 'completedIcon': true}" @click="item['completed'] = CompleteListItem(item['completed']), saveItem(item)" :icon="['fas', 'check']"></font-awesome-icon>
                            </div>
                            <div class="div2">
                                <p class="dueDate">Klart: {{item['dueDate'].replace('T', ' ')}}</p><span :class="item['priority']">{{item['priority']}}</span>
                                <p v-show="edit && !hide" class="listItem" @click="triggerItemTextEdit = keepBool(edit), hide = changeBool(hide), focusElement(item['id'])" v-bind:class="[{ 'completed': item['completed']}, SetCategoryClassForItem(item['category'])]">{{item['listItemText']}}</p>
                                <p v-show="!edit" class="listItem" @click="item['completed'] = CompleteListItem(item['completed']), saveItem(item)" v-bind:class="[{ 'completed': item['completed']}, SetCategoryClassForItem(item['category'])]">{{item['listItemText']}}</p>
                                <input placeholder="Ändra på sysslan här" :value="item['listItemText']" v-show="triggerItemTextEdit && edit" type="text" :ref="item['id']" class="editItem" name="editItem" id="editItem" @keyup.esc="triggerItemTextEdit = changeBool(triggerItemTextEdit), hide = changeBool(hide)" @keypress.enter="EditItemText(item, $event.target.value), triggerItemTextEdit = changeBool(triggerItemTextEdit), hide = changeBool(hide)"/>
                                <p class="dateString">Skapad: {{item['created'].replace('T', ' ')}}</p>
                                <select v-show="edit" class="editCategory" @change="ChangeCategoryForItem(item, $event.target.value)" name="dropdown" id="dropdown">
                                    <option selected hidden value="empty">Kategorier</option>
                                    <option v-for="obj in categories" :key="obj['id']" :value="obj['category']">{{obj['category']}}</option>
                                </select>
                                <select v-show="edit" class="" @change="EditItemPriority(item, $event.target.value)" name="dropdown" id="dropdown">
                                    <option selected hidden value="empty">Prioritet</option>
                                    <option value="Hög">Hög</option>
                                    <option value="Medel">Medel</option>
                                    <option value="Låg">Låg</option>
                                </select>
                                <input @keypress.enter="EditItemDueDate(item, $event.target.value)" v-show="edit" type="datetime-local" name="itemDueDate" id="itemDueDate">
                            </div>
                        </div>
                        <div v-show="edit">
                            <input @keypress.enter="AddItem($event.target.value), update = 1" class="addItem" placeholder="Skriv syssla eller sak"/>
                            <select class="AddItemCategory" @change="category = $event.target.value" name="dropdown" id="dropdown">
                                <option selected hidden value="empty">Kategorier</option>
                                <option v-for="obj in categories" :key="obj['id']" :value="obj['category']">{{obj['category']}}</option>
                            </select>
                            <select class="" @change="Priority = $event.target.value" name="dropdown" id="dropdown">
                                <option selected hidden value="empty">Prioritet</option>
                                <option value="Hög">Hög</option>
                                <option value="Medel">Medel</option>
                                <option value="Låg">Låg</option>
                            </select>
                            <input v-model="dueDate" type="datetime-local" name="dueDate" id="dueDate">
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
import {List1, ListItem1} from '@/models/types';
import { library, config } from '@fortawesome/fontawesome-svg-core';
import { faTimesCircle, faEdit, faCheck, faArrowUp, faArrowDown, faPlus, faMehRollingEyes} from "@fortawesome/free-solid-svg-icons";
import { faTrashAlt,faCheckCircle} from "@fortawesome/free-regular-svg-icons";
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';

library.add(faTrashAlt, faTimesCircle, faEdit, faCheckCircle, faCheck, faArrowUp, faArrowDown, faPlus );
Vue.component('font-awesome-icon', FontAwesomeIcon);


@Component
export default class Api extends Vue {
    mounted() {
        this.getUserLists();
        this.getSettings();
        console.log("mounted!!");
    };
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
        responseContainerList1 = new List1;
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

        // focusElement(elementId: string) {
        //     console.log(elementId)
        //     console.log(this.$refs)
        //     console.log(this.$refs[elementId])
        //     this.$refs[elementId].onFocus;
        // }
        
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
                    if (a.category > b.category) 
                        return 1;
                    if (a.category < b.category) 
                        return -1;
    
                    return 0;
                });
            }

            if(this.sort === 2) {
                this.List_present.listItem.sort(function(a, b) {
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

        getUserLists() {
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
                if(self.List_present === undefined)
                    self.List_present = myJson[myJson.length -1];

                if(self.List_present['UserId'] !== 0)
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
                        self.responseContainerList1 = myJson;
                        self.$store.state.z_string = myJson;
                        self.List_present = newList;
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
                        self.responseContainerList1 = myJson;
                        self.$store.state.z_string = myJson;
                        self.List_present = newList;
                        if(self.List_present !== undefined)
                            self.List_present.listItem.sort()

                        })
        };

        getUserList() {
          var self = this;
          console.log(self.$store.state.user)
          var bodyContent = { UserId: self.$store.state.user['Id'], ListId: self.responseContainerList1[self.responseContainerList1.length -1]['listId']};
          fetch('https://localhost:44366/api/values/UserList/', {
              method: 'POST',
              body: JSON.stringify(bodyContent),
              headers: {'Content-Type': 'application/json'}}).then(function(response) {
                if(response.status !== 200)
                    return self.responseIfError = "empty"
            return response.json();
            })
            .then(function(myJson) {
                console.log("here" + myJson)
                    self.List_present = myJson;
                    self.List_present.listItem.sort()
                    })
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

        EditItemText(listitem: ListItem1, newText: string){
                var self = this;
                console.log(newText);
                listitem.ListItemText = newText;
                fetch('https://localhost:44366/api/values/SaveItem/', {
                            method: 'POST',
                            body: JSON.stringify(listitem),
                            headers: {'Content-Type': 'application/json'}}).then(function() {
                                // self.get1();
                                self.getUserList();
                            });
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
                                self.getUserList();
                            });
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
                                self.getUserList();
                            });
            }
        EditListTitle(list: List1, title: string){
                var self = this;
                console.log(title);
                list.Title = title;
                fetch('https://localhost:44366/api/values/UpdateTitle/', {
                            method: 'POST',
                            body: JSON.stringify(list),
                            headers: {'Content-Type': 'application/json'}}).then(function() {
                                self.getUserLists();
                            });
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
                     self.getUserLists();
                  })
          };

        AddItem(ListItemText: string) {
              var self = this;
              alert(self.category)
              console.log(self.category)
                    self.event = ListItemText;
                    var bodyContent = {ListId: self.listid, UserId: self.userid, ListItemText: ListItemText, dueDate: self.dueDate, category: self.category, Priority: self.Priority};
                fetch('https://localhost:44366/api/values/AddItem/', {
                    method: 'POST',
                    body: JSON.stringify(bodyContent),
                    headers: {'Content-Type': 'application/json'}
                    }).then(function(){   
                        self.firstrun = 0;
                        console.log("then")
                        // self.get1();
                         self.getUserList();
                    });
                    console.log("after then")
          };

        ChangeCategoryForItem(listitem: ListItem1, newCategory: string){
              var self = this;
              console.log(newCategory)
              listitem.category = newCategory;
              fetch('https://localhost:44366/api/values/SaveItem/', {
              method: 'POST',
              body: JSON.stringify(listitem),
              headers: {'Content-Type': 'application/json'}}).then(function(){
                // self.get1();
                self.getUserList();
              });
          }

        // getAttempt() {
        //         var self = this;
        //         self.inputList = self.$store.state.test;
        //         console.log(self.inputList);

        //         fetch('https://localhost:44366/api/values/new' + '/' + self.listid + '/' + self.userid, {
        //             method: 'GET',
        //             headers: {'Content-Type': 'application/json'}
        //         }).then(function(response) {
        //             return response.json();
        //             }).then(function(myJson) {
        //                 self.items = myJson;
        //                 console.log("myJson: " + JSON.stringify(myJson));
        //             });
        //             var items = JSON.stringify("self.items: " + self.items);
        //             var list = new List1;
        //             console.log(self.items);
        //             console.log(items);

        //             return self.items;
        //     };

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
                    self.getUserListsOnDelete();
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
                 self.getUserLists();
            });
        };

}
</script>

<style scoped>
.listTitle{
    margin-bottom: 8px;
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
    color: blue;
}
.Medel {
    color: yellowgreen
}
.Hög {
    color: tomato;
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
.ListItemDiv li {
    font-weight: bolder;
    padding: 0;
    font-size: 1.3em;
    margin-left: 70%;
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
.colorscheme1 {
    color: green;
    border: green 2px solid;
}
.colorscheme2 {
    color: purple;
    border: purple 2px solid;
}
.colorscheme3 {
    color: yellow;
    border: yellow 2px solid;
}
.colorscheme4 {
    color: orange;
    border: orange 2px solid;

}
.colorscheme5 {
    color: red;
    border: red 2px solid;

}
.dueDate {
    color:palegreen;
}
.AddItemCategory{
    margin-left: 50px;
}
</style>

