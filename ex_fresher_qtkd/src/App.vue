<template>
  <div class="container">
    <the-header></the-header>
    
    <the-sidebar> </the-sidebar>
    <the-content></the-content>
    <the-footer></the-footer>
  </div>
</template>

<script>
import TheContent from './components/layout/TheContent.vue';
import TheHeader from './components/layout/TheHeader.vue';
import TheSidebar from './components/layout/TheSidebar.vue';
import TheFooter from './components/layout/TheFooter.vue';


export default {
  name: 'App',
  components: {
    TheContent,TheHeader,TheSidebar,TheFooter 
  }
}
</script>

<style>
  @import url(./css/main.css);

</style>
