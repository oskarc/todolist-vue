<template>
  <div class="create-list-container">
    <h1>Skapa Lista</h1>
    <hr>
    <!-- <div class="ContainerDivButton">
        <div>
            <p id="inputHeading">Ange användar-Id</p> <br>
            <input id="userid" type="text" v-model="userid" class="list-input" placeholder="Userid"/> <br>
        </div>
        <div>
            <p id="inputHeading">Ange List-Id</p> <br>
            <input id="listid" type="text" v-model="listid" class="list-input" placeholder="ListId"/> <br>
        </div>
    </div> -->

    <input id="title" type="text" v-model="title" class="list-title-input" placeholder="Titel"/> <button @click="createInput()">+</button>
    <div v-for="(item) in items" :id="setDivName(item)" :key="item">
        <input :id="item" class="list-item-input" placeholder="Skriv syssla eller sak"/>
        <button @click="removeLine(item)">-</button>
    </div>
    <div class="btn-container">
        <button class="buttonBlue" @click="populateList()">Spara </button>
        <!-- <button class="buttonBlue" @click="resetForm()">Rensa</button>    -->
        <p class="errorText">{{errorText}}</p>
        <p class="successText">{{successText}}</p>
    </div>
    <div v-for="obj in listList2" :key="obj['title']">
        <ul>
        <th>{{obj['title']}}</th>
        <li v-for="item in obj['list']" :key="item">{{item['ListItemText']}}</li>
    </ul>
    </div>
</div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import state from 'vuex';
import {List1, ListItem1} from '@/models/types';
import Api from './Api.vue';

@Component
export default class Create extends Vue {
    title = "";
    items= Array<string>();
    ListItem = new ListItem1;

    listItems2= new List1;
    listList2= new Array<Array<List1>>();

    iterator= 0;
    debug= "";
    removedItems= Array<string>();
    userid= 1;
    listid= 1;
    inputTemplate= "";
    errorText = "";
    successText= "";
     
        createInput () {
            var input = "input" + this.iterator;
            this.items.push(input);
            this.iterator++;
        };
        resetTitle () {
            var input = (<HTMLInputElement>document.getElementById("title")).value;
            return input = "";
        };
        resetForm() : void {
            this.items = [];
            this.title = "";
            this.iterator = 0;
            this.listList2 = new Array<Array<List1>>();
        };
        removeLine(item: string) {
            this.removedItems.push(item);
            this.debug = "item: " + item + ", indexof: " + this.items.indexOf(item);
            var itemToRemove = this.items.indexOf(item);
            this.items.splice(itemToRemove, 1);
            // document.getElementById("div" + item).remove();
        };
        setInputName(item: string) {
            var value = item;
            return value;
        };
        setDivName(index: number) {
            var value = "div" + index;
            return value;
        };
        populareListItem(listitemtext: string, Completed: boolean){
            let tempitem = new ListItem1;
            tempitem.ListItemText = listitemtext;
            tempitem.Completed = Completed;
            return tempitem;
        }
        populateList() {

            this.listItems2.UserId = this.userid;    
            this.listItems2.ListId = this.listid;
            this.listItems2.Title = this.title;
            this.listItems2.listItem = new Array<ListItem1>();

            if(this.items.length === 0)
                return this.errorText = "Lägg till fler saker"
            for (var i = 0, len = this.items.length; i < len; i++) {
             this.listItems2.listItem.push(this.populareListItem((<HTMLInputElement>document.getElementById(this.items[i])).value, false))
            }

            this.$store.state.z_arraystr = this.listList2;
            this.$store.state.z_arrayarray = this.listItems2;
            this.postAttempt();
            return "returned";
        }

        postAttempt() {

            var self = this;
            console.log(self.listItems2);

          fetch('https://localhost:44366/api/values/', {
              method: 'POST',
              body: JSON.stringify(self.listItems2),
              headers: {'Content-Type': 'application/json'}});
              this.resetForm();
              this.successText = "lista Sparad"
          };

}
</script>

<style scoped>
.list-item-input {
    margin: 4px;
    height: 15px;
}
.list-title-input {
    margin: 6px;
    width: 200px;
    height: 20px;
}
.ContainerDivButton {
    display: flex;
    justify-content: space-around;
    margin-left: 40%;
    width: 25%;
    height: 70px;
    text-align: center;

}
.ContainerDivButton input {
    margin-right: 10px;
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
    margin-top: 15px;
    }

.list-input {
  padding: 8px 12px;
  margin: 2px 0 0 0;
  box-sizing: border-box;
}
.inputHeading {
    font-family: cursive;
    font-size: 2em;
}
.errorText {
    color: #e15554;
    font-size: 2em;
    background-color: #e1bc29;
    max-width: 40%;
    margin-left: 30%;
}
.successText{
     color: #3bb273;
    font-size: 2em;
    background-color: #e1bc29;
    max-width: 40%;
    margin-left: 30%;
}
p{
    margin: 0;
}

</style>

