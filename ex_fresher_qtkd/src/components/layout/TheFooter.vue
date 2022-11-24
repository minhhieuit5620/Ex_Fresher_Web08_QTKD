<template>
    <div class="content__bottom">
                <div class="buttom__left" >
                    Tổng số:
                    <!-- <span class="total__record">{{this.sumRecord}}</span> bản ghi -->
                   <span class="total__record" v-if="!isNaN(totalRecord)" v-show="totalRecord!==null&&totalRecord!==''&&totalRecord!==undefined">{{totalRecord}}</span>  
                    <span class="total__record" v-if="(totalRecord)" v-show="totalRecord===null||totalRecord===''||totalRecord===undefined">0</span> 
                    bản ghi
                </div>
                <div class="buttom__right"  v-if="!isNaN(totalPage)">
                    <div class="rowOfPage">
                    <TheDropListPage :contentPage='contentPage' @numberRecordOfPage='numberRecordOfPage'/>
                    
                    </div>
                    <div class="page__number">
                        <div class="page__number--previous " @click="prev" v-show="!chooseOne">Trước</div>
                        <div class="page__item page__number--item first  " v-show="lastPage" @click="chooseNumber(1)" :class="{'active': choose === 1}">1</div>
                        <div class="page__number--item three__dot"  v-show="viewLast">...</div>
                        <div class="page__item page__number--item second"  @click="chooseNumber(second)" :class="{'active': choose === second}" v-show="viewFirst">{{second}}</div>
                        <div class="page__item page__number--item third" @click="chooseNumber(third)" :class="{'active': choose === third}" v-show="viewMiddle">{{third}}</div>
                        <div class="page__item page__number--item fourth" @click="chooseNumber(fourth)" :class="{'active': choose === fourth}" v-show="viewLast">{{fourth}}</div>
                        <div class="page__number--item three__dot "   v-show="viewFirst">...</div>                        
                        <!-- <div class="page__item page__number--item five" @click="chooseNumber(five)" :class="{'active': choose === five}" v-show="viewLast">{{five}}</div> -->
                        
                        <div class="page__item page__number--item last" v-show="lastPage" @click="chooseNumber(totalPage)" :class="{'active': choose === totalPage}">{{totalPage}}</div>
                        <div class="page__number--next  "  @click="next" v-show=" choose < totalPage">Sau</div>
                    </div>
                </div>
            </div>
  </template>

<script>
 /* eslint-disable */
    import TheDropListPage from'@/components/layout/TheDropList.vue';  
    import Resource from'@/js/common/resource.js';
import common from '@/js/common/common';
    export default {
        components:{TheDropListPage},
        props: {
       
        totalRecord: Number,
        totalPage:Number,
        pageNumber:Number
      
        },
        data(){
            return{
                viewFirst: true, //giao diện khi ở các trang đầu
                lastPage: true, //giao diện ẩn hiện cho trang đầu và cuối
                viewLast: false, // giao diện khi ở các trang cuối
                viewMiddle: true, //ẩn hiện cho vị trí trang ở chính giữa
                pageChoose: 1, //trang hiện tại đang chọn
                second: 2,
                third: 3,
                fourth: null,
                five:null,
                choose: this.pageNumber, //trang được chọn
                contentPage: Resource.textNumPages, //Nội dung lựa chọn số bản ghi 1 trang
                chooseOne:true,
                chooseLast:true,
                sumRecord:0,
                sumPage:0,
                totalPageValue:false,
                
        
            }
        },
       
        methods: {      

        /**
         * Chọn trang
         * author: HMHieu(26/09/2022)
         * @param {int} numberPage // số được chọn
         */
        chooseNumber(numberPage){
            this.choose = numberPage; 
            this.$emit('selectPage', this.choose);  
            if(this.choose > 1){               
                this.chooseOne=false;
            }if(this.choose === 1){
                this.chooseOne=true;
            }  
            if(this.choose < this.totalPage){         
                this.chooseLast=true;
            }
             if(this.choose === this.totalPage){
                this.chooseLast=false;
            }            
        },
        /**
         * Thay đổi giao diện khi ở giữa trang
         * author: HMHieu(26/09/2022)
         * @param {int} num 
         */
        changeViewNumberMiddle(num){
            this.second=num-1;
           // this.second = num;
            this.third = num ;
            this.fourth = num + 1;
            //this.fourth = num + 2;
        },

         /**
         * Thay đổi giao diện khi bắt đầu vào trang giữa
         * author: HMHieu(26/09/2022)
         * @param {int} num 
         */
         changeViewNumberMiddlefirst(num){
            this.second=num;
           // this.second = num;
            this.third = num+1 ;
            this.fourth = num + 2;
            //this.fourth = num + 2;
        },
         /**
         * Thay đổi giao diện khi bắt đầu vào trang giữa từ cuối
         * author: HMHieu(26/09/2022)
         * @param {int} num 
         */
         changeViewNumberMiddleLast(num){
            this.second=num-2;
           // this.second = num;
            this.third = num-1 ;
            this.fourth = num ;
            //this.fourth = num + 2;
        },

        /**Thay đổi giao diện khi ở đầu trang
         * author: HMHieu(26/09/2022)
         */
        changeViewNumberFirst(){
            this.second = 2;
            this.third = 3;
        }, 
         /**Thay đổi giao diện khi ở cuối trang
         * author: HMHieu(26/09/2022)
         */
         changeViewNumberLastAfter(){
            // this.five=this.totalPage-1;
            this.fourth = this.totalPage ;
            this.third = this.totalPage - 1;
            this.second=this.totalPage-2;
        },


        /**Thay đổi giao diện khi ở cuối trang
         * author: HMHieu(26/09/2022)
         */
        changeViewNumberLast(){
            this.fourth = this.totalPage - 1;
            this.third = this.totalPage - 2;
            //this.second=this.totalPage-3;
        },
      


        /**
         * Sự kiện ấn nút trước
         * author: HMHieu(26/09/2022)
         */
        prev(){
            if(this.choose > 1){
                this.chooseNumber(this.choose - 1);
                this.chooseOne=false;
            }if(this.choose === 1){
                this.chooseOne=true;
            }           
        },

        /**
         * Sự kiện ấn nút Sau
         * author: HMHieu(26/09/2022)
         */
        next(){
            if(this.choose < this.totalPage){    
                this.chooseNumber(this.choose + 1);     
                this.chooseLast=true;
            }
             if(this.choose === this.totalPage){
                this.chooseLast=false;
            } 
           
        },
        /**
         * Gửi số bản ghi trong 1 trang khi chọn
         * author: HMHieu(26/09/2022)
         * @param {int} num ; Số bản ghi trong 1 trang
         */
        numberRecordOfPage(num){
            this.$emit('recordOfPage', num);       
        }
         },
         watch: {
           
            pageNumber(){
                this.choose=this.pageNumber;
               
            },
        choose(newChoose){
            if(this.totalPage > 3){
                this.lastPage = true;
                if(this.totalPage===4){
                    if(newChoose==1){ 
                        this.chooseOne=true;               
                    }
                    if(newChoose <= 3){ // hiện giao diện khi ở trang đầu
                        this.viewLast = false;
                        this.viewFirst = true;
                        this.changeViewNumberFirst();
                    }
                    if(newChoose===3){
                        this.viewLast = true;
                        this.viewFirst = false;
                        this.changeViewNumberLast();
                    }
                }else{
                    if(newChoose==1){ 
                    this.chooseOne=true;               
                    }
                    if(newChoose <= 3){ // hiện giao diện khi ở trang đầu
                        this.viewLast = false;
                        this.viewFirst = true;
                        this.changeViewNumberFirst();
                    }
                    if(newChoose<=(this.totalPage-3)&&newChoose===3){

                        this.viewFirst = true; // hiện giao diện khi ở nút thứ 3 của paging
                        this.viewLast = true;
                        this.changeViewNumberMiddlefirst(newChoose)
                    }
                
                    if(newChoose <= (this.totalPage - 3) && newChoose > 3){
                        this.viewFirst = true; // hiện giao diện khi ở các trang giữa
                        this.viewLast = true;
                        this.changeViewNumberMiddle(newChoose);
                    }
                    if(newChoose > this.totalPage - 3){ // hiện giao diện khi ở các trang cuối
                        this.viewLast = true;
                        this.viewFirst = false;
                        this.changeViewNumberLast();
                    }
                    if(newChoose<=(this.totalPage-2)&&newChoose===(this.totalPage-2)){ // hiện giao diện khi ở các trang cuối
                        this.viewLast = true;
                        this.viewFirst = true;
                        this.changeViewNumberMiddleLast(newChoose);

                    }
                    if(newChoose==this.totalPage){
                        this.chooseLast=false;
                    }
                }
               
            }
        },

        totalPage(newPage){ //hiển thị lại giao diện khi số trang thay đổi
            if(newPage > 3){
                this.viewFirst = true;
                this.viewLast = false;
                this.lastPage = true;
                this.viewMiddle = true;
                this.second = 2;
                this.third = 3;
            }else{
                this.viewFirst = false;
                this.viewLast = false;
                if(newPage == 1){ //chỉ có 1 trang
                    this.lastPage = false;
                    this.viewMiddle = true;
                    this.third = 1;
                    this.chooseOne=false;
                    this.chooseLast=false;
                }
                if(newPage == 2){ // chỉ có 2 trang
                    this.viewMiddle = false;
                    this.lastPage = true;
                }
                if(newPage == 3){ // chỉ có 3 trang

                    this.viewMiddle = true;
                    this.lastPage = true;
                    this.third = 2;
                }
            }
        }
    }
    }

  </script>
  

