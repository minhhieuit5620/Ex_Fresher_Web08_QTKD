<template>
    <div class="dropList-page"    >
        <div class="dropList-text" @click="isShow =! isShow"  >{{text}}</div>
        <div class="dropList-data" v-show="isShow" v-clickoutside="hideDropList" >
            <div class="data-item" v-for="(textNumPage, index)  in contentPage"  :key="index" 
            @click="selectNumOfPage(textNumPage, index)"
            :class="{'dropList-choose': choose==index}">{{textNumPage.textData}}</div>
        </div>
        <div class="dropList-icon" @click="isShow =! isShow"  >
            <div class="dropList-icon-drop"></div>
        </div>
    </div>
</template>
    
<script>



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
        this.isShow = true; 
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
          this.isShow = false;    
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
      },
      hideDropList(){
        this.isShow = false
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
  