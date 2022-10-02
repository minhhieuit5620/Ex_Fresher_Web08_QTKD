<template>
  <div class="container">
    <the-header></the-header>
    
    <the-sidebar> </the-sidebar>
    <the-content></the-content>

  </div>
</template>

<script>
import TheContent from './components/layout/TheContent.vue';
import TheHeader from './components/layout/TheHeader.vue';
import TheSidebar from './components/layout/TheSidebar.vue';



export default {
  name: 'App',
  components: {
    TheContent,TheHeader,TheSidebar 
  }
}
</script>

<style>
  @import url(./css/main.css);

</style>
