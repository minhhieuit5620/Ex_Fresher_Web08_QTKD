<template>

    <div class="content" >
        <div class="top__content">
            <div class="top__content--title fontsize__tile">
                Nhân viên
            </div>
            <div class="top__content--add">
                <button class="btn btn__add--emp" @click="addOnClick"> Thêm mới nhân viên</button>
            </div>
        </div>
        <div class="inner__content">
            <div class="inner__top">
                <div class="inner__top--left">
                    <button class="btn btn__multiple" @click="activeMultiple" :disabled="this.listSelectedEmployee.length <2" >
                         Thực hiện hàng loạt
                         <div class="icon icon__drop--multiple"  ></div>
                    </button>
                    <button class="btn btn__delete--multiple" v-show="showMultiple"   :class="{'show__delete' : this.listSelectedEmployee.length< 1}"  @click="showPopupMultiple" >
                        Xóa 
                    </button>
                   
                </div>
                <div class="inner__top--right">
                    <div class="content__search--data">
                    <input type="text" class="input fontsize__input" 
                    v-model="searchKey"
                    @input="keySearch"
                    @keyup.enter="keySearch"
                    placeholder="Tìm kiếm theo mã, tên, số điện thoại">
                    <div class="search icon icon__search" ></div>
                </div>
                <div class="tooltip">
                    <div class="content__reload icon icon__reload" @click="reLoad">   </div>
                    <span class="tooltiptext tooltipIcon">Lấy lại dữ liệu</span>
                </div>
                <div class="tooltip">
                    <div class="content__excel icon icon__excel" @click="exportExcel"></div>
                    <span class="tooltiptext tooltipIcon">Xuất ra excel</span>
                </div>
               
                </div>
                
          
               
            </div>
            <div class="content__data">
                <table class="table">
                    <thead>
                        <tr>
                            <th class="column__sticky column__checkbox ">
                                <input class="input__checkbox" 
                                type="checkbox"
                                @click="selectAllEmployee"
                                 /> 
                            </th>
                            <th class="width__100">Mã nhân viên</th>
                            <th class="width__200">Tên nhân viên</th>
                            <th class="width__60">Giới tính</th>
                            <th class="width__100">Ngày sinh</th>
                            <th class="width__100"> <div class="tooltip">Số CMND <span class="tooltiptext tooltipAronym">Số chứng minh nhân dân</span></div> </th>                            
                            <th class="width__150">Tên đơn vị</th>
                            <th class="width__150">Chức danh</th>
                            <th class="width__100">Số tài khoản</th>
                            <th class="width__340">Tên ngân hàng</th>
                            <th class="width__250">
                                <div class="tooltip">Chi nhánh TK ngân hàng <span class="tooltiptext tooltipAronym">Chi nhánh tài khoản ngân hàng</span></div>
                            </th>
                            <th class=" column__sticky column__edit column__edit--th width__100">Chức năng</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for=" (item, index) in employee" :key="item.employeeID" 
                            @dblclick="showEditForm(item)" >
                            <td class="column__sticky column__checkbox loading__checkbox"  @dblclick.stop > 
                                <input type="checkbox" class="input__checkbox checkbox__item" 
                                :checked="isCheckedAll"                                  
                                @input="selectEmployee(item)"                               
                                />
                                <MLoadTable v-if="isLoad"/>  
                            </td >
                            <td class="width__100 text__left td__loading">
                                {{item.employeeCode}}
                                <MLoadTable v-if="isLoad"/>  
                            </td>
                            <td class="width__200 text__left td__loading">
                                {{item.employeeName}}
                                <MLoadTable  v-if="isLoad"/>  
                            </td>
                            <td class="width__60 text__left td__loading">
                                <span v-if="item.gender===this.male">Nam </span>
                                <span v-if="item.gender===this.feMale">Nữ</span>
                                <span v-if="item.gender===this.other">Khác</span> 
                                <MLoadTable  v-if="isLoad"/>  
                            </td>
                           
                            <td class="width__100 text__center td__loading">
                                {{formatDateTable(item.dateOfBirth)}}
                                <MLoadTable v-if="isLoad"/>  
                            </td>
                            <td class="width__100 text__left td__loading">
                                {{item.identityNumber}}
                                <MLoadTable v-if="isLoad"/>  
                            </td>
                            <td class="width__150 text__left td__loading">
                                {{item.departmentName}}
                                <MLoadTable v-if="isLoad"/>  
                            </td>
                            <td class="width__150 text__left td__loading">
                                {{item.positionName}}
                                <MLoadTable v-if="isLoad"/>  
                            </td>
                            <td class="width__100 text__left td__loading">
                                {{item.bankAccount}}
                                <MLoadTable v-if="isLoad"/>  
                            </td>
                            
                            <td class="width__250 text__left td__loading">
                                {{item.bankName}}
                                <MLoadTable v-if="isLoad"/>  
                            </td>
                            <td class="width__250 text__left td__loading">
                                {{item.bankBranch}}
                                <MLoadTable v-if="isLoad"/>  
                            </td>
                            <div     >
                                <div class="value__edit " v-if="isShowDropList[index]" :style="{'top': `${topDropList}px`}" >
                                    <!-- <div  v-clickoutside="hideDropList"  > -->
                                        <div class="edit__item"  @click="duplicateItem(item,index)">Nhân bản</div>
                                        <div class="edit__item" @click="showPopup(index,item.employeeID,item.employeeCode)">Xóa</div>                                
                                        <div class="edit__item" @click="changeStatus(item.employeeID,item.status,index)">
                                            <span v-if="item.status===this.work">Ngừng sử dụng </span>
                                            <span v-if="item.status===this.unWork">Sử dụng</span>                                   
                                        </div>
                                        <!-- </div> -->
                                </div>  
                             </div>  
                            <td class="edit__td column__sticky column__edit width__100 td__loading"  :ref="'row_'+index" @dblclick.stop > 
                                    <div @click="showEditForm(item)"> Sửa</div>
                                    <div class="icon__drop icon icon__edit--drop"  @click="btnShowEdit(index)" ></div>   
                                    <MLoadTable v-if="isLoad"/>                                   
                            </td>
                                                  
                        </tr>
                    </tbody>
                </table>
            </div>


            <TheFooterVue  :totalRecord="totalRecord" :totalPage="totalPage" @selectPage='selectPage' @recordOfPage='recordOfPage'/>           
        </div>
       
        <emp-detail v-if="isShow"  
        :employeeSelected="empSelected"
        :closeDialog="closeDialog"
        :formMode="formMode"
        :focus = 'focus'
        :key = 'keyForm'
        @reLoadForm = 'reLoadForm'
        @askPopUp="askPopUp"
        @waringEmail = 'waringEmail'
        @waringMaxDate='waringMaxDate'
        @closeDialog="closeDialog"
        @addOnClick="addOnClick"
        @errorEmpty='errorEmpty'
        @errorEmptyCode = 'errorEmptyCode'
        @errorEmptyName = 'errorEmptyName'
        @errorEmptyDepartment = 'errorEmptyDepartment'
        @openToast = 'openToast'
        @errorDuplicate='errorDuplicate'
        @errorBackEnd='errorBackEnd'
        ></emp-detail>        
    </div>
    <MPopup  v-if="isShowPopup" :idEmployee='idEmployee' @employeeDelete='employeeDelete' :popUpMode="popUpMode"
    @deleteMultiple="deleteMultiple" :listSelectedEmployee="listSelectedEmployee"
    @closePopup='closePopup'  :content='content' />
    <MPopupError v-show='isShowPopupError' @closeError='closeError' :text='textError'/>
    <transition name="toast-message">
        <MToastMessage v-show='isShowToast' :content = 'contentOfToastMessage' 
        @closeToastMessage='closeToastMessage' :class='{"toast-success": !isError , "toast-error": isError}' />
    </transition>
     <MLoad v-if="isLoadFull" />
     <MPopupWaringDuplicate v-show='isShowPopupDuplicate' @closeErrorDuplicate='closeErrorDuplicate' :contentDuplicate='contentDuplicate'/>

</template>
<script>
import TheFooterVue from '@/components/layout/TheFooter.vue';
import empDetail from './employeeDetail.vue';
import MPopup from '../../components/base/MPopupWaring.vue';
import MPopupError from '../../components/base/MPopupError.vue';
import eNum from '../../js/common/eNum.js';
import common from '@/js/common/common.js';
import resource from '../../js/common/resource.js';
import MToastMessage from '../../components/base/MToastMsg.vue';
import MLoadTable from '../../components/base/MLoadTable.vue';
import MLoad from '../../components/base/MLoad.vue';
import MPopupWaringDuplicate from '@/components/base/MPopupWaringDuplicate.vue';



    /**
     * URL api 
     */
    const loadDataURL =process.env.VUE_APP_URL;
    //enable debugger
    /* eslint-disable */

 /**
 * Gán sự kiện nhấn click chuột ra ngoài combobox data (ẩn data list đi)
 * HMHieu (09/10/2022)
 */
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

    name: "employeeList",
    directives: {
      clickoutside,
    },
    components: { empDetail, MPopup, MPopupError, MToastMessage, TheFooterVue, MLoadTable, MLoad, MPopupWaringDuplicate },
    created() {
        this.loadData(); 
        this.isLoad = true;
    },  
    props:{        
    },
    data() {
        return {
            employee: [],
            selected: {},
            isShow: false,
            isShowDropList: [],
            empSelected: {},
            topDropList:'',
            formMode:eNum.formMode.ADD,
            popUpMode:eNum.popUpMode.DeleteOne,
            keyForm: null,
            //loading
            isLoadFull:false,
            isLoad:false,
            //gender
            male:eNum.gender.Male,
            feMale:eNum.gender.Femail,
            other:eNum.gender.Other,
            //status
            work:eNum.statusEmployee.Woking,
            unWork:eNum.statusEmployee.UnWoking,

            totalRecord:Number,
            totalPage:Number,
            isShowPopup:false,
            idEmployee: '',
            content:'',
            isShowPopupError:false,
            textError: '',
            focus: null,
          

            isShowPopupDuplicate:false,
            contentDuplicate:'',

            isShowToast: false,
            contentOfToastMessage: '', //nội dung toast message
            isError:false, //Lỗi hay không để hiển thị toast

            pageSize: 20,
            pageIndex: 1,
            searchKey:'',
          
            // xóa nhiều
            isDisable:true,
            showMultiple:false,
            
            isCheckedAll: false,
            listSelectedEmployee: [],
        };
    },
    methods: {

        // hideDroplist(){
        // this.isShowDropList=[];
        // },
        /**
         * Hiện Form Dialog thêm mới
         * Author: HMH(16/09/22)
        */
        addOnClick() {(
            this.isShow = true,
            this.formMode = eNum.formMode.ADD,
            this.empSelected={}

        )},
         /**
         * Hàm reload lại form khi ấn cất và thêm
         * author:HMH(16/09/22)
         */
         reLoadForm(){
            try {
                this.formMode = eNum.formMode.ADD,
                this.empSelected = {};
                this.keyForm = Math.floor(Math.random()*90000);
            } catch (error) {
                console.log(error)
            }
        },
        /**
         * Ẩn Form Dialog
         * Author: HMH(14/09/22)
        */
        closeDialog() {
            this.isShow = false;
            this.loadData();
        },  
        /**
         * Ẩn hiện dropList ở cột sửa
         * Author: HMH(15/09/22)
         * @param {*} index vị trí element được chọn
         */             
        btnShowEdit(index) {                  
            this.isShowDropList[index] = !this.isShowDropList[index];//ẩn hiện dropList
            let [row] = this.$refs['row_'+index];//get element được chọn
            this.topDropList = row.getBoundingClientRect().top + 35; //getBoundingClientRect dùng để lấy vị trí của element             
        },
        /**
         * 
         * @param {*} index 
         * Ẩn edit list
         * Author: HMHieu(18/09/22)
         */
        // hideListData(index){
        //     this.isShowDropList =false;
        // },

        /**
         * Hiển thị nhân viên khi dbclick để sửa
         * Author: HMHieu(18/09/22)
         */
        showEditForm(data){           
            this.isShow = true;      
            this.formMode=eNum.formMode.Edit;     
            this.empSelected=data;            
        }, 
        /**
         * format date hiển thị list danh sách nhân viên
         * Author: HMHieu(18/09/22)
         */
        formatDateTable(date){
           
            try {
            let dateFormat = common.formatDateTable(date);
            return dateFormat;
            } catch (error) {
                console.log(error);
            }
        },    
       reLoad(){
        this.loadData();
        this.isLoad = true;
       },


        /**
         * Lấy toàn bộ nhân viên và tổng số bản ghi hiện có
         * Author: HMHieu(18/09/22)
         */
        loadData() {          
            fetch(loadDataURL+`Employees/filter?search=${this.searchKey}&pageIndex=${this.pageIndex}&pageSize=${this.pageSize}`, { method: "GET" })
                .then((res) => res.json())
                .then((data) => {                
                    this.employee = data.data;                   
                    this.totalRecord=data.totalCount;
                    this.totalPage=data.totalPage;
                    setTimeout(() => (this.isLoad = false), 500);      
                    setTimeout(() => (this.isLoadFull = false), 500);                    
                })
                .catch((res) => {
                    console.log(res);            
                });
        },
        /**
         * Hàm load lại table khi chọn trnag mới
         * author: HMHieu(26/09/2022)
         * @param {int} page : Trang được chọn
         */
         selectPage(page){
            this.pageIndex = page;
            this.loadData();
        },              

        /**
         * Hàm chọn số bản ghi trong 1 trang
         * author: HMHieu(26/09/2022)
         * @param {int} numRecord : Sô bản ghi trong 1 trang
         */
        recordOfPage(numRecord){
            this.pageSize = numRecord;
            this.loadData();
        },

        /**
         * Hiện popUp  chức năng xóa nhân viên 
         * Author: HMHieu(19/09/22)
         */
        showPopup(index,id,code){
            this.isShowPopup = true;
            this.idEmployee=id;      
            this.content= `Bạn có thực sự muốn xóa nhân viên <${code}> không?` ;
            this.isShowDropList[index] = !this.isShowDropList[index];//ẩn hiện dropList
            this.popUpMode=eNum.popUpMode.DeleteOne;
        },
        closeDropList(){
            console.log('Clicked');
            this.isShowDropList=[];
        },
        /**
         * ẩn droplist action
         */
        hideDropList(){
            debugger;
         this.closeDropList();
            //this.isShowDropList[index] = !this.isShowDropList[index];//ẩn hiện dropList
        },
        /**
         * Hiện popUp  chức năng xóa nhiều nhân viên 
         * Author: HMHieu(06/10/22)
         */
        showPopupMultiple(){
            this.isShowPopup = true;        
            this.content= `Bạn có thực sự muốn xóa những nhân viên đã chọn không?` ;
            this.popUpMode=eNum.popUpMode.DeleteMultiple;           
        },
        /**
         * Đóng form chức năng xóa nhân viên 
         * Author: HMHieu(19/09/22)
         */
        closePopup(){
            this.isShowPopup = false;    
            this.showMultiple=false; 
        },

        /**
         * Hàm hiện cảnh báo mã nhân viên / tên/ đơn vị trống
         * author:  HMHieu(21/09/22)
         */
         errorEmpty(msg){
            try {
                this.isShowPopupError = true;
                switch(msg){
                    case resource.popupError.EmptyCode.name:
                        this.textError = resource.popupError.EmptyCode.content;                       
                    break;
                    case resource.popupError.EmptyName.name:
                        this.textError =resource.popupError.EmptyName.content;
                    break;
                    case resource.popupError.EmptyDepartment.name:
                        this.textError = resource.popupError.EmptyDepartment.content;
                    break;                   
                }
            } 
            catch (error) {
                console.log(error);
            }
           
        },
          /**
         * Hàm hiện cảnh báo lỗi backend
         * author:  HMHieu(21/09/22)
         */
         errorBackEnd(sts,content){
            try {
                this.isShowPopup = false; 
                this.isShowPopupError = true; 
                 if(this.formMode===eNum.formMode.ADD||this.formMode===eNum.formMode.Edit||this.formMode===eNum.formMode.Duplicate){
                    if(sts===eNum.errorBackEnd.User){
                        this.textError = resource.popupErrorBackend.User.content;
                    }
                    else if(sts===eNum.errorBackEnd.NotFound){
                        this.textError = resource.popupErrorBackend.NotFound.content;
                    }
                    else if(sts===eNum.errorBackEnd.Ser){
                        this.textError = resource.popupErrorBackend.Server.content;
                    }
                     else if(sts===eNum.errorBackEnd.InsertFailCode){
                        this.textError = content;
                    }
                    else if(sts===eNum.errorBackEnd.exception){
                        this.textError = resource.popupErrorBackend.User.content;
                    }else{
                        this.textError = resource.popupErrorBackend.Server.content;
                    }
                 }else{
                    if(sts===eNum.errorBackEnd.User){
                        this.textError = resource.popupErrorBackend.Deleted.content;
                    }
                    else if(sts===eNum.errorBackEnd.NotFound){
                        this.textError = resource.popupErrorBackend.NotFound.content;
                    }
                    else if(sts===eNum.errorBackEnd.Ser){
                        this.textError = resource.popupErrorBackend.Server.content;
                    }
                     else if(sts===eNum.errorBackEnd.InsertFailCode){
                        this.textError = resource.popupErrorBackend.User.content;
                    }
                    else if(sts===eNum.errorBackEnd.exception){
                        this.textError = resource.popupErrorBackend.User.content;
                    }else{
                        this.textError = resource.popupErrorBackend.Server.content;
                    }
                 }
                  
                    // else{
                    //     this.isShowPopupError = false;                       
                    //     this.closeDialog();
                    //     this.isLoadFull=true                      
                    //     this.loadData();
                    //     this.openToast(resource.ToastMessage.success);
                    // }           
            } 
            catch (error) {
                console.log(error);
            }
           
        },
        /**
         * 
         * @param {*} employeeCode 
         * PopUp trùng mã nhân viên
         * Author: HMHieu(09/10/22)
         */
        errorDuplicate(employeeCode){
            if(employeeCode === '' || employeeCode == undefined || employeeCode == null){
                this.isShowPopupDuplicate=true;
                this.contentDuplicate=`Mã nhân viên không hợp lệ đã tồn tại trong hệ thống, vui lòng kiểm tra lại.`;
            
            }else{
                this.isShowPopupDuplicate=true;
                this.contentDuplicate=`Mã nhân viên <${employeeCode}>đã tồn tại trong hệ thống, vui lòng kiểm tra lại.`;
            }
           
        },
         /**        
         * PopUp cảnh báo email không đúng định dạng
         * Author: HMHieu(09/10/22)
         */
         waringEmail(){         
            this.isShowPopupDuplicate=true;
            this.contentDuplicate='Email chưa đúng định dạng';
        },
          /**        
         * PopUp cảnh báo ngày nhập quá ngày hiện tại
         * Author: HMHieu(09/10/22)
         */
         waringMaxDate(){         
            this.isShowPopupDuplicate=true;
            this.contentDuplicate='Ngày nhập vào không quá ngày hiện tại';
        },
          /**
         * Đóng Popup error duplicate
        * Author: HMHieu(21/09/22)
         */
         closeErrorDuplicate(){
            this.isShowPopupDuplicate=false;
            this.focus =! this.focus;
        },
      

        /**
         * Đóng Popup error
        * Author: HMHieu(21/09/22)
         */
        closeError(){
            this.isShowPopupError = false;
            this.focus =! this.focus;
        },
          /**
         * Hàm đóng toastmessage
         * author: HMHieu(25/9/2022)
         */
         closeToastMessage(){
            this.isShowToast = false;
        },
         /**
         * Hàm mở toastmessage
         * author: HMHieu(25/9/2022)
         */
         openToast(msg){ 
            let me=this;
            me.contentOfToastMessage = msg;
            me.isShowToast = true;
            if(msg === resource.ToastMessage.success){
                me.isError = false;
            }
               
            if(msg === resource.ToastMessage.error){ 
                me.isError = true;
            }         
            setTimeout(function(){
                me.isShowToast = false;
            }, 1200);
        },

        /**
         * 
         * @param {*} event 
         */
        keySearch(event){
            let me=this;
            this.pageIndex=1;
            try{
              //  let key=event.keyCode;

                if(event.keyCode===eNum.KeyCode.Enter){
                fetch(loadDataURL+`Employees/filter?search=${this.searchKey}&pageIndex=${this.pageIndex}&pageSize=${this.pageSize}`, { method: "GET" })
                .then((res) => res.json())
                .then((data) => {
                    me.employee = data.data;                   
                    me.totalRecord=data.totalCount;
                    me.totalPage=data.totalPage;
                    setTimeout(() => (this.isLoad = false), 500);      
                    setTimeout(() => (this.isLoadFull = false), 500);                    
                })
                .catch((res) => {
                    this.errorBackEnd( eNum.errorBackEnd.Ser);                    
                    console.log(res);            
                });
                }
                              
            }
            catch(error){
                console.log(error);
            }                              
        },

         /**
         * Xóa nhân viên theo ID
         * Author: HMHieu(19/09/22)
         */
        employeeDelete(idEmployee){          
            // var me = this;
            var method = "Delete";             
            var DeleteURL= loadDataURL+`Employees/${idEmployee}`;                 
            // Xóa dữ liệu:            
            fetch(DeleteURL, {                
                method: method,
                         
            })
                .then((res) => res.json())
                .then((res) => {                                                     
                    let errorCode = res.errorCode;  
                    let ok=res.success;
                    if(ok){
                        this.isShowPopup = false;  
                        this.isLoadFull=true;
                        setTimeout(() => (this.isLoadFull=true), 500);     
                        this.openToast(resource.ToastMessage.success);
                        this.loadData();  
                    }else{
                        if (
                            errorCode === eNum.errorBackEnd.InsertFailCode||
                            errorCode === eNum.errorBackEnd.exception)
                        {
                            this.errorBackEnd(errorCode);                                                     
                        }   
                    }
                                                     
                })
                .catch((res) => {
                    this.errorBackEnd( eNum.errorBackEnd.Ser);                    
                    console.log(res);                 
                    this.openToast(resource.ToastMessage.error);                               
                });
        },     

          /**
         * Đổi trạng thái làm việc của nhân viên
         * Author: HMHieu(01/10/22)
         */
         changeStatus(idEmployee,employeeStatus,index){          
            // var me = this;
            var method = "PUT";     
            var  changeURL=""
            if(employeeStatus===eNum.statusEmployee.UnWoking){
                changeURL= loadDataURL+`Employees/change-status?statusEmployee=${eNum.statusEmployee.Woking} &EmployeeID=${idEmployee}`;  
            }
            else{
                changeURL= loadDataURL+`Employees/change-status?statusEmployee=${eNum.statusEmployee.UnWoking} &EmployeeID=${idEmployee}`;  
            }                                     
            // thay đổi trạng thái dữ liệu:            
            fetch(changeURL, {                
                method: method, 
                headers: {
                        "Content-Type": "application/json",
                    },
                    body: JSON.stringify(employeeStatus)                  
            })
                .then((res) => res.json())
                .then((res) => {
                    let ok=res.success;
                    let errorCode=res.errorCode;
                    if(ok){
                        console.log(res);
                        this.isShowDropList[index] = !this.isShowDropList[index];//ẩn hiện dropList   
                        this.loadData();  
                        this.openToast(resource.ToastMessage.success);  
                    }
                    if( errorCode === eNum.errorBackEnd.InsertFailCode||
                        errorCode === eNum.errorBackEnd.exception)
                    {
                        this.errorBackEnd(errorCode)                                                     
                    }

                      
                                     
                })
                .catch((res) => {
                    console.log(res);   
                    this.errorBackEnd( eNum.errorBackEnd.Ser);                                  
                    this.openToast(resource.ToastMessage.error);                               
                });
        },   
          /**
         * Nhân đôi thông tin của nhân viên
         * Author: HMHieu(01/10/22)
         */
        duplicateItem(data,index){

           // console.log(data);
            this.isShow = true,
            this.formMode = eNum.formMode.Duplicate,
            this.empSelected=data
            this.isShowDropList[index] = !this.isShowDropList[index];//ẩn hiện dropList
        },
        /**
         * Xuất dữ liệu ra file excel
         * Author: HMHieu(01/10/22)
         */
        exportExcel(){                       
            var  changeURL=loadDataURL+`Employees/export-excel`                                                                                      
            fetch(changeURL)
                .then((res) => { return res.blob(); })
                .then((data) => {
                var a = document.createElement("a");
                a.href = window.URL.createObjectURL(data);
                a.download = "Danh_sach_nhan_vien.xlsx";
                a.click();           
                this.loadData();              
            }).catch((res) => {
                    this.errorBackEnd( eNum.errorBackEnd.Ser); 
                    console.log(res);                 
                    this.openToast(resource.ToastMessage.error);                               
                });            
        } ,
        
      /**
       * ẩn hiện button xóa
       * Author: HMHieu(04-10-22)
       */
        activeMultiple(){
            if (this.listSelectedEmployee.length  > 1) {
                this.showMultiple=!this.showMultiple;                
            }
            if(this.listSelectedEmployee.length  <1){
                this.showMultiple=false;
            }                    
        },       
       
        // Xử lý sự kiện click vào input checkbox nhân viên
        // Author: HMHieu(04-10-22)
        selectEmployee(data) {
            if (!this.listSelectedEmployee.includes(data.employeeID)) {
                this.listSelectedEmployee.push(data.employeeID);            
                
            } else {
                this.listSelectedEmployee.splice(
                    this.listSelectedEmployee.indexOf(this.employeeID) + 1,
                    1
                );
                this.showMultiple=false;
            } 
            console.log(this.listSelectedEmployee);               
        },

         // Xử lý sự kiện click vào input checkbox tổng
        // Author: HMHieu  04-10-22
        selectAllEmployee() {
            this.isCheckedAll = !this.isCheckedAll;
            if (
                this.listSelectedEmployee.length != this.employee.length ||
                (this.listSelectedEmployee.length == this.employee.length &&
                    this.isCheckedAll)
            ) {
                this.employee.forEach((e) => {
                    if (!this.listSelectedEmployee.includes(e.employeeID))
                        this.listSelectedEmployee.push(e.employeeID);
                });
            } else {
                this.listSelectedEmployee = [];
                this.showMultiple=false;
            }
            console.log(this.listSelectedEmployee);                   
        },

        /**
         * Chức năng xóa nhiều nhân viên
         * CreatedBy: HMHieu (04-10-22)
         */
        deleteMultiple(listEmplyeeDelete){
            const me=this;
            var method = "POST";
                fetch(loadDataURL+'Employees/delete-multiple', {                
                method: method,
                headers: {
                "Content-Type": "application/json",
                },
                body: JSON.stringify(listEmplyeeDelete),
                 })
                .then((res) => res.json())
                .then((res) => {    
                    let ok=res.success;
                    let errorCode=res.errorCode;
                    let totalDeleted=res.data;
                    if(ok){
                        //this.$emit('openToast', resource.ToastMessage.success);
                        this.isShowPopup = false;  
                        this.isLoadFull=true;
                        setTimeout(() => (this.isLoadFull=true), 500);                             
                        me.openToast(totalDeleted+resource.ToastMessage.successDeleteMultiple);  
                        // me.$emit("closeDialog");
                        me.loadData();   
                        me.showMultiple=false; 
                    }  
                    if( errorCode === eNum.errorBackEnd.InsertFailCode||
                        errorCode === eNum.errorBackEnd.exception)
                    {
                        this.errorBackEnd(errorCode)                                                     
                    }                                              
                    // if(errorCode===eNum.errorBackEnd.Ser||status===eNum.errorBackEnd.User||status===eNum.errorBackEnd.NotFound){
                    //         me.errorBackEnd(status);    
                    //      //   me.$emit('errorBackEnd',status);                           
                    // }
                   
                    this.listSelectedEmployee=[];                 
                })
                .catch((res) => {
                    this.errorBackEnd( eNum.errorBackEnd.Ser);                    
                    console.log(res);
                    this.openToast(resource.ToastMessage.error);  
                   
                });
        }

    }
}
</script>
<style>

</style>