<template>
    <div class="dropList-page" >
        <div class="dropList-text">{{text}}</div>
        <div class="dropList-data" v-show="isShow">
            <div class="data-item" v-for="(textNumPage, index) in contentPage" :key="index" 
            @click="selectNumOfPage(textNumPage, index)"
            :class="{'dropList-choose': choose==index}">{{textNumPage.textData}}</div>
        </div>
        <div class="dropList-icon" @click="isShow =! isShow">
            <div class="dropList-icon-drop"></div>
        </div>
    </div>
</template>
    
<script>

  export default {

    props: {
      contentPage: Array // Mảng chứa lựa chọn số lượng bản ghi
    },
    created(){
      var recorInSession=sessionStorage.getItem("recordPage");
      var textpage=sessionStorage.getItem("textPage");
      var index=sessionStorage.getItem("index");
        if(recorInSession!=null || recorInSession!=undefined){
          this.$emit("numberRecordOfPage", recorInSession);
          this.text =textpage;
          this.choose=index;
        }
    },
    methods: { 
      selectNumOfPage(textNumPage, index){
        var recorInSession=sessionStorage.getItem("recordPage");
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
     },
      addPage(record,textPage,index){
        if(record==""||record==undefined){
          this.recordInpage=20;          
          sessionStorage.setItem("recordPage",this.recordInpage);
          sessionStorage.setItem("textPage",textPage);
          sessionStorage.setItem("index",index);
        }else{
          sessionStorage.setItem("recordPage",record);
          sessionStorage.setItem("textPage",textPage);
          sessionStorage.setItem("index",index);
        }
        console.log(this.textPage);
      }
    },
    updated(){
      var recorInSession=sessionStorage.getItem("recordPage");
      var textpage=sessionStorage.getItem("textPage");
      var index=sessionStorage.getItem("index");
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

        choose: null,
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
  