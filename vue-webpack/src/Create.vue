<template>
<div class="create-list-container">
    <h1>Skapa Lista</h1>
    <!-- <span>listList: {{listList}} </span> <br>
    <span>listItems: {{listItems}} </span> <br>
    <span>iterator: {{iterator}} </span> <br>
    <span>items: {{items}} </span> <br>
    <span>debug: {{debug}} </span> <br>
    <span>removedItems: {{removedItems}} </span> <br>
    <span>areas: {{areas}}</span> -->
    <hr>
    <span id="inputHeading">Ange användar-Id</span> <br>
    <input id="userid" type="text" v-model="userid" class="list-title-input" placeholder="Userid"/> <br>
    <input id="title" type="text" v-model="title" class="list-title-input" placeholder="Titel"/> <button @click="createInput()">+</button>
    <div v-for="(item) in items" :id="setDivName(item)" :key="item">
        <component :id="item" :is="addInput"></component>
        <button @click="removeLine(item)">-</button>
    </div>
    <div class="btn-container">
        <button @click="populateList()">spara lista</button>
        <button @click="resetForm()">Ta allt</button>   
    </div>
    <div v-for="obj in listList" :key="obj['title']">
        <ul>
        <th>{{obj['title']}}</th>
        <li v-for="item in obj['list']" :key="item">{{item['ListItemText']}}</li>
    </ul>
    </div>
</div>
</template>

<script>
import {state} from 'vuex';
import {myVar, settings} from './Globals.js';
export default {
    data: function() {
        return {
            title: '',
            items: [],
            listItems: [],
            listList: [],
            iterator: 0,
            debug: "",
            removedItems: [],
            userid: 0
        }
    },
    methods: {
        createInput: function() {
            this.items.push("input" + (this.iterator));
            this.iterator++;
        },
        resetTitle: function() {
            var input = document.getElementById("title");
            return input.innerText ="";
        },
        resetForm: function() {
            this.items = [];
            this.title = "";
            this.iterator = 0;
            this.listList = [];
        },
        removeLine: function(item) {
            this.removedItems.push(item);
            this.debug = "item: " + item + ", indexof: " + this.items.indexOf(item);
            var itemToRemove = this.items.indexOf(item);
            this.items.splice(itemToRemove, 1);
            // document.getElementById("div" + item).remove();
        },
        setInputName: function(item) {
            var value = item;
            return value;
        },
        setDivName: function(index) {
            var value = "div" + index;
            return value;
        },
        populateList: function() {
            this.listItems = [];

            this.listItems = { UserId: this.userid, ListId: 1, title: this.title, list: [] };
            var isDone = "completed: false";
            this.items.forEach(
                item => this.listItems.list.push({ListItemText: document.getElementById(item).value, Completed: false })
            );

            if(this.listList == [{}])
                this.listList[{}] = this.listItems;
            else {
                this.listList[this.listList.length] = this.listItems;
            }

            return this.$store.state.test = this.listList;
        }
    },
    computed: {
        addInput () {
            return {
                template: '<input class="list-item-input" placeholder="Skriv syssla eller sak"></input>'
            }
        },
        areas () {
            return this.$store.state.test;;
        }
    }
}
</script>

<style scoped>
#inputHeading{
    margin: 20px;
}
.create-list-container {
    text-align: left;
}
.list-title-input {
    margin: 10px;
}
.list-item-input {
    margin: 10px;
}
.btn-container {
    margin-left: 20px;
}
</style>