<template>
  <div class="home">
    <div class="container">
    <div id="one"></div>
    <div id="two">
        <div id="three"><span>qwekwpqokfwoeijrpginegpiwnrepigwnepirngpiwenrgpienrgpinwrpnpignwpireng</span></div>
        <div id="four"><span>+oqkwd0åaje+åfgwrågjerjgåeijrgojewogejroigjerojgeorijgoijreoi</span></div>
    </div>
</div>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import HelloWorld from '@/components/HelloWorld.vue'; // @ is an alias to /src
import Create from '@/views/Create.vue';

@Component({
  components: {
    HelloWorld,
    Create,
  },
})
export default class Home extends Vue {

}
</script>

<style scoped>
* {
  margin: 0px;
  padding: 0px;
  box-sizing: border-box;
}

.container {
  width: 100%;
}

#one {
  height: 300px;
  width: 300px;
  background-color: green;
  float: left;
  margin-left: 25%;
}

#two {
  display: block;
  max-height: 300px;
  max-width: 500px;
  margin-left: 10%;
}

#three {
  display: inline-block;
  height: 200px;
  width: 200px;;
  background-color: yellow;
}

#four {
  display: inline-block;
  height: 200px;
  width: 200px;
  background-color: blue;
}
</style>

