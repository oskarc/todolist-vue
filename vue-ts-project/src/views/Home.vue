<template>
  <div class="home">
    <HelloWorld msg="Welcome to Your Vue.js + TypeScript App"/>
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
