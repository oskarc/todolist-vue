<template>
  <div class="home">
    
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

</style>

