<template>

    <div class="content">
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
                <div class="content__search--data">
                    <input type="text" class="input fontsize__input" 
                    v-model="searchKey"
                    @input="loadData"
                    @keyup.enter="loadData"
                    placeholder="Tìm kiếm theo mã, tên nhân viên">
                    <div class="search icon icon__search" ></div>
                </div>
                <div class="content__reload icon icon__reload" @click="loadData"></div>
            </div>
            <div class="content__data">
                <table class="table">
                    <thead>
                        <tr>
                            <th class="column__sticky column__checkbox "><input class="input__checkbox" type="checkbox" /> </th>
                            <th class="width__150">Mã nhân viên</th>
                            <th class="width__200">Tên nhân viên</th>
                            <th class="width__100">Giới tính</th>
                            <th class="width__100">Ngày sinh</th>
                            <th class="width__100"> <div class="tooltip">Số CMND <span class="tooltiptext tooltipAronym">Số chứng minh nhân dân</span></div> </th>                            
                            <th class="width__150">Tên đơn vị</th>
                            <th class="width__150">Chức danh</th>
                            <th class="width__100">Số tài khoản</th>
                            <th class="width__250">Tên ngân hàng</th>
                            <th class="width__300">
                                <div class="tooltip">Chi nhánh TK ngân hàng <span class="tooltiptext tooltipAronym">Chi nhánh tài khoản ngân hàng</span></div>
                            </th>
                            <th class=" column__sticky column__edit column__edit--th">Chức năng</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for=" (item, index) in employee" :key="item.employeeID" 
                            @dblclick="showEditForm(item)" >
                            <td class="column__sticky column__checkbox"  @dblclick.stop > <input type="checkbox" class="input__checkbox checkbox__item" /></td >
                            <td class="width__150">{{item.employeeCode}}</td>
                            <td class="width__200">{{item.employeeName}}</td>
                            <td class="width__100">
                                <span v-if="item.gender===this.male">Nam </span>
                                <span v-if="item.gender===this.feMale">Nữ</span>
                                <span v-if="item.gender===this.other">Khác</span>   
                            </td>
                           
                            <td class="width__100">{{formatDateTable(item.dateOfBirth)}}</td>
                            <td class="width__100">{{item.identityNumber}}</td>
                            <td class="width__150">{{item.departmentName}}</td>
                            <td class="width__150">{{item.positionName}}</td>
                            <td class="width__100">{{item.bankAccount}}</td>
                            
                            <td class="width__250">{{item.bankName}}</td>
                            <td class="width__250">{{item.bankBranch}}</td>
                         
                           
                     
                          
                            
                            <div class="value__edit"  v-show="isShowDropList[index]" :style="{'top': `${topDropList}px`}" >
                                    <div class="edit__item">Nhân bản</div>
                                    <div class="edit__item" @click="showPopup(index,item.employeeID)">Xóa</div>                                
                                    <div class="edit__item" @click="changeStatus(item.employeeID,item.status,index)">
                                        <span v-if="item.status===this.work">Ngừng sử dụng </span>
                                        <span v-if="item.status===this.unWork">Sử dụng</span>                                   
                                    </div>
                            </div>    
                            <td class="edit__td column__sticky column__edit width__100"  :ref="'row_'+index" @dblclick.stop > 
                                    <div @click="showEditForm(item)"> Sửa</div>
                                    <div class="icon__drop icon icon__edit--drop"  @click="btnShowEdit(index)" ></div>                                    
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
        @closeDialog="closeDialog"
        @errorEmpty='errorEmpty'
        @errorEmptyCode = 'errorEmptyCode'
        @errorEmptyName = 'errorEmptyName'
        @errorEmptyDepartment = 'errorEmptyDepartment'
        @openToast = 'openToast'

        @errorBackEnd='errorBackEnd'
        ></emp-detail>        
    </div>
    <MPopup  v-if="isShowPopup" :idEmployee='idEmployee' @employeeDelete='employeeDelete'
    @closePopup='closePopup'  :content="'Bạn có thực sự muốn xóa nhân viên này? '" />
    <MPopupError v-show='isShowPopupError' @closeError='closeError' :text='textError'/>
    <transition name="toast-message">
        <MToastMessage v-show='isShowToast' :content = 'contentOfToastMessage' 
        @closeToastMessage='closeToastMessage' :class='{"toast-success": !isError , "toast-error": isError}' />
    </transition>
   
</template>
<script>
import TheFooterVue from '@/components/layout/TheFooter.vue';
import empDetail from './employeeDetail.vue';
import MPopup from '../../components/base/MPopupWaring.vue';
import MPopupError from '../../components/base/MPopupError.vue';
import eNum from '../../js/common/eNum.js';
import common from '@/js/common/common.js';
import resource from '../../js/common/resource.js';
import MToastMessage from '../../components/base/ToastMsg.vue';

    /**
     * URL api 
     */
    var loadDataURL =process.env.VUE_APP_URL;

export default {

    name: "employeeList",
    components: { empDetail,MPopup,MPopupError,MToastMessage,TheFooterVue},
    created() {
        this.loadData(); 
       
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
            isShowPopupError:false,
            textError: '',

            isShowToast: false,
            contentOfToastMessage: '', //nội dung toast message
            isError:false, //Lỗi hay không để hiển thị toast

            pageSize: 20,
            pageIndex: 1,
            searchKey:'',
          
        };
    },
    methods: {
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
                    this.totalPage=data.totalPage                           
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
         * Đóng form chức năng xóa nhân viên 
         * Author: HMHieu(19/09/22)
         */
        showPopup(index,id){
            this.isShowPopup = true;
            this.idEmployee=id;         
            this.isShowDropList[index] = !this.isShowDropList[index];//ẩn hiện dropList
        },
        /**
         * Đóng form chức năng xóa nhân viên 
         * Author: HMHieu(19/09/22)
         */
        closePopup(){
            this.isShowPopup = false;            
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
         errorBackEnd(sts){
            try {
                this.isShowPopup = false; 
                this.isShowPopupError = true; 
                 
                   if(sts===eNum.errorBackEnd.User){
                        this.textError = resource.popupErrorBackend.User.content;
                    }
                    else if(sts===eNum.errorBackEnd.NotFound){
                        this.textError = resource.popupErrorBackend.NotFound.content;
                    }
                    else if(sts===eNum.errorBackEnd.Ser){
                        this.textError = resource.popupErrorBackend.Server.content;
                    }
                    else{
                        this.isShowPopupError = false; 
                        this.openToast(resource.ToastMessage.success);
                        this.closeDialog();
                        this.loadData();
                    }           
            } 
            catch (error) {
                console.log(error);
            }
           
        },
        /**
         * Đóng Popup error
        * Author: HMHieu(21/09/22)
         */
        closeError(){
            this.isShowPopupError = false;
           
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
                    var status = res.status;              
                    if (status === eNum.errorBackEnd.Ser||eNum.errorBackEnd.User||eNum.errorBackEnd.NotFound)
                    {
                        this.errorBackEnd(status);                                                     
                    }                   
                    else {                              
                        this.isShowPopup = false;                 
                        this.loadData();                                                                                                                                                                             
                    }
                })
                .catch((res) => {
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
            // Xóa dữ liệu:            
            fetch(changeURL, {                
                method: method, 
                headers: {
                        "Content-Type": "application/json",
                    },
                    body: JSON.stringify(employeeStatus)                  
            })
                .then((res) => res.json())
                .then((res) => {     
                     console.log(res);
                    this.isShowDropList[index] = !this.isShowDropList[index];//ẩn hiện dropList   
                    this.loadData();  
                    this.openToast(resource.ToastMessage.success);                   
                })
                .catch((res) => {
                    console.log(res);                 
                    this.openToast(resource.ToastMessage.error);                               
                });
        },   
        duplicateItem(){
            
        }  
    }
}
</script>
<style>

</style>