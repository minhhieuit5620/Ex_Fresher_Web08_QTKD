<template>
    <div class="dropList-page"   >
        <div class="dropList-text" @click="openDropList"  >{{text}}</div>
        <div class="dropList-data" v-show="isShow"  v-clickoutside="hideDropList" >
            <div class="data-item" v-for="(textNumPage, index)  in contentPage"  :key="index" 
            @click="selectNumOfPage(textNumPage, index)"
            :class="{'dropList-choose': choose==index}">{{textNumPage.textData}}</div>
        </div>
        <div class="dropList-icon"  @click="openDropList"  >
            <div class="dropList-icon-drop" ></div>
        </div>
    </div>
</template>
    
<script>
 /* eslint-disable */
import Resource from '@/js/common/resource';



const clickoutside = {
  mounted(el, binding) {
    el.clickOutsideEvent = (event) => {
      //click ra ngoài
      // event: đối tượng click
      if (
        !(( el === event.target || // click phạm vi của combobox-value
            el.previousElementSibling.contains(event.target)
          )) //click vào ele trước combobox-value
        ) 
      {
        binding.value();
      }
    };
    document.body.addEventListener("click", el.clickOutsideEvent);
  },
  beforeUnmount: (el) => {
    document.body.removeEventListener("click", el.clickOutsideEvent);
  },
};
  export default {
    
    props: {
      contentPage: Array // Mảng chứa lựa chọn số lượng bản ghi
    },
    directives: {
      clickoutside,
    },
    created(){
      var recorInSession=sessionStorage.getItem(Resource.Session.recordPage);
      var textpage=sessionStorage.getItem(Resource.Session.textPage);
      var index=sessionStorage.getItem(Resource.Session.index);
        if(recorInSession!=null || recorInSession!=undefined){
          this.$emit("numberRecordOfPage", recorInSession);
          this.text =textpage;
          this.choose=index;
        }
    },
    methods: { 
      selectNumOfPage(textNumPage, index){
        this.isShow = true; 
        var recorInSession=sessionStorage.getItem(Resource.Session.recordPage);
        // var recorInSession=sessionStorage.getItem("recordPage");
          if(recorInSession!=null || recorInSession!=undefined){
            this.choose = index;
            this.text = textNumPage.textData;
            this.textPage=textNumPage.textData;
            this.indexPage=index;
            this.addPage(textNumPage.num, this.textPage,this.indexPage);
            this.$emit("numberRecordOfPage", recorInSession);            
            this.isShow = false;     
          }else{
            this.choose = index;
            this.text = textNumPage.textData;
            this.textPage=textNumPage.textData;
            this.indexPage=index;            
            this.$emit("numberRecordOfPage", textNumPage.num);
            this.addPage(textNumPage.num, this.textPage,this.indexPage);
            this.isShow = false;     
          }
          this.isShow = false;    
     },
      addPage(record,textPage,index){
        if(record==""||record==undefined){
          this.recordInpage=20;          
          sessionStorage.setItem(Resource.Session.recordPage,this.recordInpage);
          sessionStorage.setItem(Resource.Session.textPage,textPage);
          sessionStorage.setItem(Resource.Session.index,index);
        }else{
          sessionStorage.setItem(Resource.Session.recordPage,record);
          sessionStorage.setItem(Resource.Session.textPage,textPage);
          sessionStorage.setItem(Resource.Session.index,index);
          
        }
        console.log(this.textPage);
      },
      hideDropList(){
        this.isShow = false
        },
      openDropList(){       
        this.isShow= !this.isShow
      }
    },

    updated(){
      var recorInSession=sessionStorage.getItem(Resource.Session.recordPage);
      var textpage=sessionStorage.getItem(Resource.Session.textPage);
      var index=sessionStorage.getItem(Resource.Session.index);   
        if(recorInSession!=null || recorInSession!=undefined){
          this.$emit("numberRecordOfPage", recorInSession);
          this.text =textpage;
          this.choose=index;
           
        }
        
    },
    data(){
      return{
        isShow: false,
        textNumPages: null,

        choose: 1,
        text: '20 bản ghi trên một trang',

        recordInpage:null,
        textPage:"",
        indexPage:null

      }
    }
  }
</script>
  
  <!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
 
</style>
  