<template>
    <div class="container">


        <the-sidebar> </the-sidebar>

        <the-header></the-header>
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
                    <div class="inner__top--left">
                        <button class="btn btn__multiple" @click="activeMultiple"
                            :disabled="this.listSelectedEmployee.length < 2">
                            Thực hiện hàng loạt
                            <div class="icon icon__drop--multiple"></div>
                        </button>
                        <button class="btn btn__delete--multiple" v-show="showMultiple"
                            :class="{ 'show__delete': this.listSelectedEmployee.length < 1 }"
                            @click="showPopupMultiple">
                            Xóa
                        </button>

                    </div>
                    <div class="inner__top--right">
                        <div class="content__search--data">
                            <input type="text" class="input fontsize__input" v-model="searchKey" @input="keySearch"
                                @keyup.enter="keySearch" placeholder="Tìm kiếm theo mã, tên, số điện thoại">
                            <div class="search icon icon__search"></div>
                        </div>
                        <div class="tooltip">
                            <div class="content__reload icon icon__reload" @click="reLoad"> </div>
                            <span class="tooltiptext tooltipIcon">Lấy lại dữ liệu</span>
                        </div>
                        <div class="tooltip" v-show="showExport">
                            <div class="content__excel icon icon__excel" @click="exportExcel"></div>
                            <span class="tooltiptext tooltipIcon">Xuất ra excel</span>
                        </div>
                        <!-- <div class="tooltip" v-show="showExport">
                            <div class="content__excel icon icon__import--default" @click="clickChosseFile"></div>                           
                            <input type="file" @change="importDefault" ref="file" class="chosse__file--hide">                            
                            <span class="tooltiptext tooltipIcon">Nhập khẩu</span>
                        </div> -->
                        <div class="tooltip">

                            <div class="content__excel icon icon__import" @click="openImportEx"></div>

                            <!-- <input type="file" ref="inputImport" class="content__excel icon icon__import"  @change="importExcel($event)"> -->
                            <span class="tooltiptext tooltipIcon">Nhập khẩu</span>
                        </div>

                    </div>



                </div>
                <div class="content__data">
                    <table class="table">
                        <thead>
                            <tr>
                                <th class="column__sticky column__checkbox ">
                                    <input class="input__checkbox" type="checkbox" :checked="isCheckedAll"
                                        @input="selectAllEmployee(employee)" />
                                </th>
                                <th class="width__150">Mã nhân viên</th>
                                <th class="width__200">Tên nhân viên</th>
                                <th class="width__100">Giới tính</th>
                                <th class="width__100 text__center">Ngày sinh</th>
                                <th class="width__100">
                                    <div class="tooltip">Số CMND <span class="tooltiptext tooltipAronym">Số chứng minh
                                            nhân dân</span></div>
                                </th>
                                <th class="width__150">Tên đơn vị</th>
                                <th class="width__150">Chức danh</th>
                                <th class="width__100">Số tài khoản</th>
                                <th class="width__340">Tên ngân hàng</th>
                                <th class="width__250">
                                    <div class="tooltip">Chi nhánh TK ngân hàng <span
                                            class="tooltiptext tooltipAronym">Chi nhánh tài khoản ngân hàng</span></div>
                                </th>
                                <th class=" column__sticky column__edit column__edit--th width__100">Chức năng</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr v-for=" (item, index) in employee" :key="item.employeeID"
                                @dblclick="showEditForm(item)">
                                <td class="column__sticky column__checkbox loading__checkbox" @dblclick.stop>
                                    <input type="checkbox" class="input__checkbox checkbox__item"
                                        :checked="listSelectedEmployee.some((obj) => obj == item.employeeID)"
                                        @input="selectEmployee(item)" />
                                    <MLoadTable v-if="isLoad" />
                                </td>
                                <td class="width__150 text__left td__loading">
                                    {{ item.employeeCode }}
                                    <MLoadTable v-if="isLoad" />
                                </td>
                                <td class="width__200 text__left td__loading">
                                    {{ item.employeeName }}
                                    <MLoadTable v-if="isLoad" />
                                </td>
                                <td class="width__100 text__left td__loading">
                                    <span v-if="item.gender === this.male">Nam </span>
                                    <span v-if="item.gender === this.feMale">Nữ</span>
                                    <span v-if="item.gender === this.other">Khác</span>
                                    <MLoadTable v-if="isLoad" />
                                </td>

                                <td class="width__100 text__center td__loading">
                                    {{ formatDateTable(item.dateOfBirth) }}
                                    <MLoadTable v-if="isLoad" />
                                </td>
                                <td class="width__100 text__left td__loading">
                                    {{ item.identityNumber }}
                                    <MLoadTable v-if="isLoad" />
                                </td>
                                <td class="width__150 text__left td__loading">
                                    {{ item.departmentName }}
                                    <MLoadTable v-if="isLoad" />
                                </td>
                                <td class="width__150 text__left td__loading">
                                    {{ item.positionName }}
                                    <MLoadTable v-if="isLoad" />
                                </td>
                                <td class="width__100 text__left td__loading">
                                    {{ item.bankAccount }}
                                    <MLoadTable v-if="isLoad" />
                                </td>

                                <td class="width__250 text__left td__loading">
                                    {{ item.bankName }}
                                    <MLoadTable v-if="isLoad" />
                                </td>
                                <td class="width__250 text__left td__loading">
                                    {{ item.bankBranch }}
                                    <MLoadTable v-if="isLoad" />
                                </td>
                                <div>
                                    <div class="value__edit " v-if="isShowDropList[index]"  
                                        :style="{ 'top': `${topDropList}px` }">
                                        <!-- <div  v-clickoutside="hideDropList"  > -->
                                        <div class="edit__item" @click="duplicateItem(item, index)">Nhân bản</div>
                                        <div class="edit__item"
                                            @click="showPopup(index, item.employeeID, item.employeeCode)">Xóa</div>
                                        <div class="edit__item"
                                            @click="changeStatus(item.employeeID, item.status, index)">
                                            <span v-if="item.status === this.work">Ngừng sử dụng </span>
                                            <span v-if="item.status === this.unWork">Sử dụng</span>
                                        </div>
                                        <!-- </div> -->
                                    </div>
                                </div>
                                <td class="edit__td column__sticky column__edit width__100 td__loading"
                                    :ref="'row_' + index" @dblclick.stop>
                                    <div @click="showEditForm(item)" class="text__edit"> Sửa</div>
                                    <div class="icon__drop icon icon__edit--drop" ref="openDroplist" @click="btnShowEdit(index)"></div>
                                    <MLoadTable v-if="isLoad" />
                                </td>

                            </tr>
                        </tbody>
                    </table>
                </div>


                <TheFooterVue :totalRecord="totalRecord" :pageNumber="pageIndex" :totalPage="totalPage"
                    @selectPage='selectPage' @recordOfPage='recordOfPage' />
            </div>

            <emp-detail v-if="isShow" :employeeSelected="empSelected" :closeDialog="closeDialog" :formMode="formMode"
                :focus='focus' :key='keyForm' @reLoadForm='reLoadForm' @askPopUp="askPopUp" @warningEmail='warningEmail'
                @warningMaxDateOfBirth='warningMaxDateOfBirth'
                @warningMaxDateIdentity='warningMaxDateIdentity'
                 @warningMinIdenDate='warningMinIdenDate' @closeDialog="closeDialog"
                @addOnClick="addOnClick" @errorEmpty='errorEmpty' @errorEmptyCode='errorEmptyCode'
                @errorEmptyName='errorEmptyName' @errorEmptyDepartment='errorEmptyDepartment' @openToast='openToast'
                @errorDuplicate='errorDuplicate' @errorBackEnd='errorBackEnd' @loadData='loadData'></emp-detail>
            <importExcel v-if="showImport" :closeImportEx="closeImportEx" @errorEmptyFile='errorEmptyFile'
                @errorTypeFile="errorTypeFile" @noRecordValid="noRecordValid" @errorBackEnd='errorBackEnd'
                @loadData='loadData' @showPopupClose="showPopupClose">

            </importExcel>
        </div>
        <MPopup v-if="isShowPopup" :idEmployee='idEmployee' @employeeDelete='employeeDelete' :popUpMode="popUpMode"
            @deleteMultiple="deleteMultiple" :listSelectedEmployee="listSelectedEmployee" @closeImportEx="closeImportEx"
            @closePopup='closePopup' :content='content' />
        <MPopupError v-show='isShowPopupError' @closeError='closeError' :text='textError' />
        <transition name="toast-message">
            <MToastMessage v-show='isShowToast' :content='contentOfToastMessage' @closeToastMessage='closeToastMessage'
                :class='{ "toast-success": !isError, "toast-error": isError }' />
        </transition>
        <MLoad v-if="isLoadFull" />
        <MPopupWaringDuplicate v-show='isShowPopupDuplicate' @closeErrorDuplicate='closeErrorDuplicate' :popUpModeOneBtn="popUpModeOneBtn"
            :contentDuplicate='contentDuplicate'  @logOutAccount="logOutAccount"/>
    </div>
</template>
<script>
import empDetail from './EmployeeDetail.vue';
import ImportExcel from './ImportData.vue';
import TheFooterVue from '@/components/layout/TheFooter.vue';
import MPopup from '../../components/base/MPopupWaring.vue';
import MPopupError from '../../components/base/MPopupError.vue';
import eNum from '../../js/common/eNum.js';
import common from '@/js/common/common.js';
import resource from '../../js/common/resource.js';
import MToastMessage from '../../components/base/MToastMsg.vue';
import MLoadTable from '../../components/base/MLoadTable.vue';
import MLoad from '../../components/base/MLoad.vue';
import MPopupWaringDuplicate from '@/components/base/MPopupWaringDuplicate.vue';
import baseApi from '@/js/common/baseApi';
import TheHeader from '../../components/layout/TheHeader.vue';
import TheSidebar from '../../components/layout/TheSidebar.vue';
import Resource from '../../js/common/resource.js';
import urlApi from '@/js/common/urlApi';



/* eslint-disable */

/**
* Gán sự kiện nhấn click chuột ra ngoài combobox data (ẩn data list đi)
* HMHieu (09/10/2022)
*/
const clickoutside = {
    mounted(el, binding) {
        debugger;     
        el.clickOutsideEvent = (event) => {
            //click ra ngoài
            // event: đối tượng click
            if (
                !((el === event.target || // click phạm vi của combobox-value
                    el.contains(event.target)
                )) &&(el===this.$refs.openDroplist) //click vào ele trước combobox-value
            ) {
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
    components: {
        TheHeader,
        TheSidebar,
        empDetail,
        ImportExcel,
        MPopup,
        MPopupError,
        MToastMessage,
        TheFooterVue,
        MLoadTable,
        MLoad,
        MPopupWaringDuplicate,

    },
    created() {
        this.loadData();
        this.isLoad = true;
        
        // this.refreshToken();
    },
    mounted() {
        this.showExportExcel();
    },
    beforeUpdate() {

    },
    props: {
    },
    data() {
        return {
            showImport: false,
            employee: [],
            selected: {},
            isShow: false,
            isShowDropList: [],
            empSelected: {},
            topDropList: '',
            formMode: eNum.formMode.ADD,
            popUpMode: eNum.popUpMode.DeleteOne,
            popUpModeOneBtn:eNum.popUpModeOneBtn.warning,
            keyForm: null,
            //loading
            isLoadFull: false,
            isLoad: false,
            //gender
            male: eNum.gender.Male,
            feMale: eNum.gender.Femail,
            other: eNum.gender.Other,
            //status
            work: eNum.statusEmployee.Woking,
            unWork: eNum.statusEmployee.UnWoking,

            totalRecord: Number,
            totalPage: Number,
            isShowPopup: false,
            idEmployee: '',
            content: '',
            isShowPopupError: false,
            textError: '',
            focus: null,


            isShowPopupDuplicate: false,
            contentDuplicate: '',

            isShowToast: false,
            contentOfToastMessage: '', //nội dung toast message
            isError: false, //Lỗi hay không để hiển thị toast

            //page
            pageSize: eNum.page.pageSizeDefault,
            pageIndex:eNum.page.pageIndexDefault,                     
            searchKey: '',

            // xóa nhiều
            isDisable: true,
            showMultiple: false,

            isCheckedAll: false,
            listSelectedEmployee: [],
            timeOutSearch: null,
            showExport: false,
        };
    },
    methods: {

        /********************
        * Mở form nhập khẩu
        * HMHieu 15-11-2022
        * 
        */
        openImportEx() {
            this.showImport = true;
        },
        /********************
        * đóng form nhập khẩu excel
        * HMHieu 15-11-2022
        * 
        */
        closeImportEx() {
            this.showImport = false;
            this.loadData();
        },

        /********************
        * làm mới token nếu đã đăng nhập
        * HMHieu 04-11-2022
        * 
        */
        // refreshToken() {
        //     var accessToken = localStorage.getItem(Resource.Manager.token.accessToken);
        //     var refreshToken = localStorage.getItem(Resource.Manager.token.refreshToken);
        //     if (common.checkEmptyData(accessToken) || common.checkEmptyData(refreshToken)) {
        //         //nếu không thì chuyển sang trang login
        //         this.$router.push('/login');
        //     }
        //     else {

        //         //nếu có tồn tại token
        //         authToken.refreshToken(accessToken, refreshToken);
        //     }

        // },

        /**
         * Ẩn hiện nút xuất excel
         * Hiện nếu role =Admin/ ẩn nếu role=User
         */
        showExportExcel() {
            //let token = localStorage.getItem(resource.Manager.token.accessToken);
            let rol = localStorage.getItem(resource.Manager.token.rol);           
            if(!common.checkEmptyData(rol)){
                for (let index = 0; index < rol.length; index++) {
                    if (rol[index] === resource.Manager.role.admin) {                       
                        this.showExport = true;
                    }
                    else if (rol === resource.Manager.role.admin) {                       
                        this.showExport = true;
                    }
                    else {
                        this.showExport = false;
                    }
                
                }
            }else{
                this.showExport = false;
            }
            
            //var role = baseApi.parseJwt(token);
            
        },
        // hideDroplist(){
        // this.isShowDropList=[];
        // },
        /**
         * Hiện Form Dialog thêm mới
         * Author: HMH(16/09/22)
        */
        addOnClick() {
            (
                this.isShow = true,
                this.formMode = eNum.formMode.ADD,
                this.empSelected = {}

            )
        },
        /**
        * Hàm reload lại form khi ấn cất và thêm
        * author:HMH(16/09/22)
        */
        reLoadForm() {
            try {
                this.formMode = eNum.formMode.ADD,
                    this.empSelected = {};
                this.keyForm = Math.floor(Math.random() * eNum.timeOut.waitRadom);
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
            let [row] = this.$refs['row_' + index];//get element được chọn
            this.topDropList = row.getBoundingClientRect().top + eNum.marginTop.marginTopEdit; //getBoundingClientRect dùng để lấy vị trí của element             
        },      

        /**
         * Hiển thị nhân viên khi dbclick để sửa
         * Author: HMHieu(18/09/22)
         */
       async showEditForm(data) {
            try {
                 
                let method=resource.method.GET;         
                var URL =urlApi.methodCRUD.getEmployeeById
                var urlGet=URL.replace("#",data.employeeID)
                try {
                    var res= await baseApi.fetchAPIDefaultV2(urlGet,method) ;                    
                } catch (error) {
                    this.errorBackEnd(eNum.errorBackEnd.Ser);
                    console.log(error);

                }                
                    if(res.ok){
                        this.empSelected = await res.json();
                        this.isShow = true;
                        this.formMode = eNum.formMode.Edit;
                    }else{
                        let status=res.status;
                        if(status===eNum.errorBackEnd.User){
                            this.errorBackEnd(eNum.errorBackEnd.User);                            
                        }else{
                            this.errorBackEnd(eNum.errorBackEnd.Ser);

                        }
                    }                                     
               
            } catch (error) {
                this.errorBackEnd(eNum.errorFE.UserActionError);
                console.log(error);
            }
           

        },
        /**
         * format date hiển thị list danh sách nhân viên
         * Author: HMHieu(18/09/22)
         */
        formatDateTable(date) {         
            let dateFormat = common.formatDateTable(date);
            return dateFormat;          
        },
        /**
         * Hàm lấy lại dữ liệu
         */
        reLoad() {
            this.loadData();
            this.isLoad = true;
        },

        /**
         * Lấy toàn bộ nhân viên và tổng số bản ghi hiện có
         * Author: HMHieu(18/09/22)
         */
        async loadData() {
            //this.pageIndex=1;            
            try {
                let url = urlApi.apiGet(this.searchKey, this.pageIndex, this.pageSize);
                let method = Resource.method.GET;             
                    var response = await baseApi.fetchAPIDefault(url, method);
                    this.employee = response.data;
                    if (this.employee.length === 0) {
                        this.pageIndex = 1;
                        url = urlApi.apiGet(this.searchKey, this.pageIndex, this.pageSize)
                        response = await baseApi.fetchAPIDefault(url, method);
                        this.employee = response.data;
                        // this.selectPage(this.pageIndex);
                    }                          
                //kiểm tra trạng thái checkbox của header table
                this.checkedHeader(this.employee)
                this.totalRecord = response.totalCount;
                this.totalPage = response.totalPage;
                setTimeout(() => (this.isLoad = false), eNum.timeOut.loadData);
                setTimeout(() => (this.isLoadFull = false), eNum.timeOut.loadData);

            } catch (error) {
                var token=localStorage.getItem(Resource.Manager.token.accessToken);

                if(token===null||token === '' || token === "undefined"||token === undefined){
                    this.showPopupLogout();                  
                }else{
                    this.errorBackEnd(eNum.errorBackEnd.Ser);  

                }                                          
                console.log(error);
            }

        },
        /**
         * Hàm load lại table khi chọn trnag mới
         * author: HMHieu(26/09/2022)
         * @param {int} page : Trang được chọn
         */
        selectPage(page) {             
            this.pageIndex = page;
            this.loadData();
        },

        /**
         * Hàm chọn số bản ghi trong 1 trang
         * author: HMHieu(26/09/2022)
         * @param {int} numRecord : Sô bản ghi trong 1 trang
         */
        recordOfPage(numRecord) {
            this.pageSize = numRecord;
            this.loadData();
        },

        /**
         * Hiện popUp  chức năng xóa nhân viên 
         * Author: HMHieu(19/09/22)
         */
        showPopup(index, id, code) {
            this.isShowPopup = true;
            this.idEmployee = id;
            this.content =Resource.DeleteOne.DeleteOneBefore+code+Resource.DeleteOne.DeleteOneAfter
            this.isShowDropList[index] = !this.isShowDropList[index];//ẩn hiện dropList
            this.popUpMode = eNum.popUpMode.DeleteOne;
        },

        /********************
        * Khi bị thay đổi thông tin(token==rỗng hoặc undifined) sẽ hiện thông báo để đăng xuất
        * HMHieu 19-11-2022
        * 
        */
        showPopupLogout(){
            this.isShowPopupDuplicate = true;
            this.contentDuplicate = resource.PopupWarning.ErrorAccount;
            
            this.popUpModeOneBtn=eNum.popUpModeOneBtn.logout;
        },

        //đăng xuất
        logOutAccount(){
            localStorage.clear();
            this.$router.push('/login');
        },

        /********************
        * Popup hiển thị khi đang trong quá trình nhập khẩu dữ liệu người dùng
        * HMHieu 15-11-2022
        * 
        */
        showPopupClose() {
            this.isShowPopup = true;

            this.content = Resource.PopupWarning.CloseImport;

            this.popUpMode = eNum.popUpMode.CloseImport;
        },
        /**
         * Hàm đóng droplist Action
         * 
         */
        closeDropList() {
            //console.log('Clicked');
            this.isShowDropList = [];
        },
        /**
         * ẩn droplist action
         */
        hideDropList() {

            this.isShowDropList = [];

            //this.isShowDropList[index] = !this.isShowDropList[index];//ẩn hiện dropList
        },
        /**
         * Hiện popUp  chức năng xóa nhiều nhân viên 
         * Author: HMHieu(06/10/22)
         */
        showPopupMultiple() {
            this.isShowPopup = true;
            this.content = Resource.PopupWarning.CloseDeleteMul;
            this.popUpMode = eNum.popUpMode.DeleteMultiple;
        },
        /**
         * Đóng form chức năng xóa nhân viên 
         * Author: HMHieu(19/09/22)
         */
        closePopup() {
            this.isShowPopup = false;
            this.showMultiple = false;
        },

        /**
         * Cảnh báo nếu chưa chọn file
         */
        errorEmptyFile() {
            this.isShowPopupDuplicate = true;
            this.contentDuplicate = resource.popupError.EmptyFile.content;
            this.popUpModeOneBtn=eNum.popUpModeOneBtn.warning;

        },
        /**
      * Cảnh báo nếu chưa chọn file
      */
        errorTypeFile() {
            this.isShowPopupDuplicate = true;
            this.contentDuplicate = resource.popupError.ErrorFile.content;
            this.popUpModeOneBtn=eNum.popUpModeOneBtn.warning;

        },
        /**
      * Cảnh báo khi chọn bước 3 import mà không có bản ghi nào hợp lệ
      */
        noRecordValid() {
            this.isShowPopupDuplicate = true;
            this.contentDuplicate = resource.popupError.ErrorNextStep3.content;
            this.popUpModeOneBtn=eNum.popUpModeOneBtn.warning;

        },

        /**
         * Hàm hiện cảnh báo mã nhân viên / tên/ đơn vị trống
         * author:  HMHieu(21/09/22)
         */
        errorEmpty(msg) {
            try {
                this.isShowPopupError = true;
                switch (msg) {
                    case resource.popupError.EmptyCode.name:
                        this.textError = resource.popupError.EmptyCode.content;
                        break;
                    case resource.popupError.EmptyName.name:
                        this.textError = resource.popupError.EmptyName.content;
                        break;
                    case resource.popupError.EmptyDepartment.name:
                        this.textError = resource.popupError.EmptyDepartment.content;
                        break;
                    case resource.popupError.ErrorMaxCode.name:
                        this.textError = resource.popupError.ErrorMaxCode.content;
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
        errorBackEnd(sts, content) {
            try {
                this.isShowPopup = false;
                this.isShowPopupError = true;
                if (this.formMode === eNum.formMode.ADD || this.formMode === eNum.formMode.Edit || this.formMode === eNum.formMode.Duplicate) {
                    if (sts === eNum.errorBackEnd.User) {//lỗi người dùng theo trạng thái
                        this.textError = resource.popupErrorBackend.User.content;
                    }
                    else if (sts === eNum.errorBackEnd.NotFound) {// lỗi không tìm thấy trang(404)
                        this.textError = resource.popupErrorBackend.NotFound.content;
                    }
                    else if (sts === eNum.errorBackEnd.Ser) {//lỗi do server (500)
                        this.textError = resource.popupErrorBackend.Server.content;
                    }
                    else if (sts === eNum.errorBackEnd.InsertFailCode) {//mã code lỗi do backend quy định
                        this.textError = content;//nội dung lỗi do BE trả về
                    }
                    else if (sts === eNum.errorBackEnd.exception) {//Mã code lỗi do gặp ex BE
                        this.textError = resource.popupErrorBackend.User.content;
                    }
                    else if (sts === eNum.errorBackEnd.notFileImport) {//Mã code lỗi do gặp ex BE
                        this.textError = resource.popupErrorBackend.NotExitsFile.content;
                    }
                    else if(sts===eNum.errorFE.UserActionError){
                        this.textError=resource.popupError.UserActionError.content;
                    }
                    else {
                        this.textError = resource.popupErrorBackend.Server.content;
                    }
                } else {
                    if (sts === eNum.errorBackEnd.User) {
                        this.textError = resource.popupErrorBackend.Deleted.content;
                    }
                    else if (sts === eNum.errorBackEnd.NotFound) {
                        this.textError = resource.popupErrorBackend.NotFound.content;
                    }
                    else if (sts === eNum.errorBackEnd.Ser) {
                        this.textError = resource.popupErrorBackend.Server.content;
                    }
                    else if (sts === eNum.errorBackEnd.InsertFailCode) {
                        this.textError = resource.popupErrorBackend.User.content;
                    }
                    else if (sts === eNum.errorBackEnd.exception) {
                        this.textError = resource.popupErrorBackend.User.content;
                    } else {
                        this.textError = resource.popupErrorBackend.Server.content;
                    }
                }
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
        errorDuplicate(employeeCode) {
            if (employeeCode === '' || employeeCode == undefined || employeeCode == null) {
                this.isShowPopupDuplicate = true;
                this.contentDuplicate = Resource.PopupWarning.EmptyCode;
                this.popUpModeOneBtn=eNum.popUpModeOneBtn.warning;


            } else {
                this.isShowPopupDuplicate = true;
                this.contentDuplicate = Resource.Duplicate.DuplicateBefore+employeeCode+Resource.Duplicate.DuplicateAfter;
                this.popUpModeOneBtn=eNum.popUpModeOneBtn.warning;

            }

        },
        /**        
        * PopUp cảnh báo email không đúng định dạng
        * Author: HMHieu(09/10/22)
        */
        warningEmail() {
            this.isShowPopupDuplicate = true;
            this.contentDuplicate = Resource.PopupWarning.ErrorEmailFormat;
            this.popUpModeOneBtn=eNum.popUpModeOneBtn.warning;

        },
        /**        
       * PopUp cảnh báo ngày cấp quá ngày hiện tại
       * Author: HMHieu(09/10/22)
       */
        warningMaxDateIdentity() {
            this.isShowPopupDuplicate = true;
            this.contentDuplicate = Resource.PopupWarning.ErrorMaxDateIdentity;
            this.popUpModeOneBtn=eNum.popUpModeOneBtn.warning;

        },
          /**        
       * PopUp cảnh báo ngày sinh quá ngày hiện tại
       * Author: HMHieu(09/10/22)
       */
       warningMaxDateOfBirth() {
            this.isShowPopupDuplicate = true;
            this.contentDuplicate = Resource.PopupWarning.ErrorMaxDateOfBirth;
            this.popUpModeOneBtn=eNum.popUpModeOneBtn.warning;

        },
        /**        
       * PopUp cảnh báo ngày sinh lớn hơn ngày cấp
       * Author: HMHieu(09/10/22)
       */
        warningMinIdenDate() {
            this.isShowPopupDuplicate = true;
            this.contentDuplicate = Resource.PopupWarning.MinDateIdentity;
            this.popUpModeOneBtn=eNum.popUpModeOneBtn.warning;

        },
        /**
       * Đóng Popup error duplicate
      * Author: HMHieu(21/09/22)
       */
        closeErrorDuplicate() {
            this.isShowPopupDuplicate = false;
            this.focus = !this.focus;
        },


        /**
         * Đóng Popup error
        * Author: HMHieu(21/09/22)
         */
        closeError() {
            this.isShowPopupError = false;
            this.focus = !this.focus;
        },
        /**
       * Hàm đóng toastmessage
       * author: HMHieu(25/9/2022)
       */
        closeToastMessage() {
            this.isShowToast = false;
        },
        /**
        * Hàm mở toastmessage
        * author: HMHieu(25/9/2022)
        */
        openToast(msg) {
            let me = this;
            me.contentOfToastMessage = msg;
            me.isShowToast = true;
            if (msg === resource.ToastMessage.success) {
                me.isError = false;
            }

            if (msg === resource.ToastMessage.error) {
                me.isError = true;
            }
            setTimeout(function () {
                me.isShowToast = false;
            }, eNum.timeOut.waitToast);
        },

        /**
         * 
         * @param {*} event 
         * hàm tìm kiếm
         */
        async keySearch(event) {
            this.pageIndex = eNum.page.pageIndexDefault;

            try {
                //  let key=event.keyCode;

                if (event.keyCode === eNum.KeyCode.Enter) { //sử dụng phím Enter để search
                    let url = urlApi.apiGet(this.searchKey, this.pageIndex, this.pageSize);
                    //let url=loadDataURL+`Employees/filter?search=${this.searchKey}&pageIndex=${this.pageIndex}&pageSize=${this.pageSize}`;
                    let method = Resource.method.GET;
                    try {
                        var response = await baseApi.fetchAPIDefault(url, method);//call API
                        this.employee = response.data;
                        this.totalRecord = response.totalCount;// tổng số bản ghi
                        this.totalPage = response.totalPage;// tổng số trang
                        setTimeout(() => (this.isLoad = false), eNum.timeOut.loadData);
                        setTimeout(() => (this.isLoadFull = false), eNum.timeOut.loadData);
                    } catch (error) {
                        this.errorBackEnd(eNum.errorBackEnd.Ser);
                        console.log(error);
                    }
                 

                }
                else { // xét time để search sau khi người dùng gõ vào ô tìm kiếm 2 giây

                    try {
                        if (this.timeOutSearch) {// Khi người dùng đang gõ
                        clearTimeout(this.timeOutSearch)
                        this.timeOutSearch = null;
                        }

                        this.timeOutSearch = setTimeout(() => (this.loadData()), eNum.timeOut.loadSearch);   // khi người dùng gõ xong  
                    } catch (error) {
                        this.errorBackEnd(eNum.errorBackEnd.Ser);
                        console.log(error);
                    }
                                                                               
                }
            }
            catch (error) {                
                this.errorBackEnd(eNum.errorFE.UserActionError);
                console.log(error);
            }
        },

        /**
        * Xóa nhân viên theo ID
        * Author: HMHieu(19/09/22)
        */
        async employeeDelete(idEmployee) {
            var me = this;
            try {
                var method = Resource.method.DELETE;
                var DeleteURL = urlApi.apiDeleteOrPut(idEmployee)                          
                // Xóa dữ liệu: 
                try {
                    var response = await baseApi.fetchAPIDefault(DeleteURL, method);
                    this.handleResponseDefault(response, me);    
                } catch (error) {
                    this.errorBackEnd(eNum.errorBackEnd.Ser);
                    console.log(error);
                }
                

            } catch (error) {
                this.errorBackEnd(eNum.errorFE.UserActionError);
                console.log(error);
            }

        },
        /**
         * Hàm xử lý dữ liệu trả về từ BE khi Get, Delete one
         * @param {*} res response
         * @param {*} me //con trỏ
         * CreatedBy: HMHieu (24-10-2022)
         */
        handleResponseDefault(res, me) {
            let errorCode = res.errorCode;
            let ok = res.success;
            if (ok) {//thành công
                me.isShowPopup = false;
                me.isLoadFull = true;
                setTimeout(() => (me.isLoadFull = true), eNum.timeOut.loadData);  //show load   
                me.openToast(resource.ToastMessage.success);
                me.loadData();
            } else {//Thất bại 
                if (
                    errorCode === eNum.errorBackEnd.InsertFailCode ||
                    errorCode === eNum.errorBackEnd.exception) {//bắt mã lỗi trả về từ BE
                    me.errorBackEnd(errorCode);
                }
            }
        },


        /**
       * Đổi trạng thái làm việc của nhân viên
       * Author: HMHieu(01/10/22)
       */
        async changeStatus(idEmployee, employeeStatus, index) {
            try {

                var me = this;
                var method = Resource.method.PUT;
                var changeURL = ""
                if (employeeStatus === eNum.statusEmployee.UnWoking) {//kiểm tra trạng thái hiện tại của nhân viên                
                    changeURL = urlApi.apiChangeStatus(eNum.statusEmployee.Woking, idEmployee)
                }
                else {
                    changeURL = urlApi.apiChangeStatus(eNum.statusEmployee.UnWoking, idEmployee)                     
                }
                try {
                     // thay đổi trạng thái dữ liệu:      
                    var response = await baseApi.fetchAPIBody(employeeStatus, changeURL, method)
                    this.isShowDropList[index] = !this.isShowDropList[index];//ẩn hiện dropList
                    this.handleResponseDefault(response, me); //truyền về hàm handle Response
                } catch (error) {
                    this.errorBackEnd(eNum.errorBackEnd.Ser);
                    console.log(error);
                }
               

            } catch (error) {
                this.errorBackEnd(eNum.errorFE.UserActionError);
                console.log(error);
            }

        },
        /**
       * Nhân đôi thông tin của nhân viên
       * Author: HMHieu(01/10/22)
       */
        duplicateItem(data, index) {

            // console.log(data);
            this.isShow = true,
                this.formMode = eNum.formMode.Duplicate,
                this.empSelected = data
            this.isShowDropList[index] = !this.isShowDropList[index];//ẩn hiện dropList
        },
        /**
         * Xuất dữ liệu ra file excel
         * Author: HMHieu(01/10/22)
         */
        exportExcel() {
            try {
                var token = localStorage.getItem(Resource.Manager.token.accessToken);               
                let check = common.checkEmptyData(token);
                if (check) {
                    this.$router.push('/login');
                } else {
                    var changeURL = urlApi.apiExportExcel(this.searchKey);
                    // loadDataURL+`Employees/export-excel?search=${this.searchKey}`; 
                    // window.location.href=changeURL;                                                                                   
                    fetch(changeURL, {
                        headers: {
                            "Content-Type": "application/json",
                            "Authorization": `Bearer ${token}`
                        }
                    })
                        .then((res) => { return res.blob(); })
                        .then((data) => {
                            var a = document.createElement("a");
                            a.href = window.URL.createObjectURL(data);
                            a.download = Resource.NameFileDownLoad.NameFile;
                            a.click();
                            this.loadData();
                        }).catch((res) => {
                            this.errorBackEnd(eNum.errorBackEnd.Ser);
                            console.log(res);
                            this.openToast(resource.ToastMessage.error);
                        });
                }

            } catch (error) {
                this.errorBackEnd(eNum.errorFE.UserActionError);
                console.log(error);
            }

        },

        /**
         * Chọn file
         */
        clickChosseFile() {
            this.$refs.file.click();
            //this.nameFile=this.$refs.file.files[0].name;   
        },

        /**
         * Nhập khẩu dữ liệu một bước
         * 
         */
        async importDefault() {
            try {
                //chọn file
                let file = this.$refs.file.files[0];
                var fileImport = new FormData();
                fileImport.append('file', file)

                let method = Resource.method.POST;
               
                try {
                    var res = await baseApi.fetchAPIFile(urlApi.importDefault.import, fileImport, method);
                    if (res.ok) {
                        this.loadData();
                        this.openToast(resource.ToastMessage.success);
                    } else {
                        this.openToast(resource.ToastMessage.error)
                    }
                } catch (error) {
                    this.errorBackEnd(eNum.errorBackEnd.Ser);
                    console.log(error);
                }            
            } catch (error) {
                this.errorBackEnd(eNum.errorFE.UserActionError);
                console.log(error);
            }

        },


        /**
        * ẩn hiện button xóa
        * Author: HMHieu(04-10-22)
        */
        activeMultiple() {
            if (this.listSelectedEmployee.length > 1) {
                this.showMultiple = !this.showMultiple;
            }
            if (this.listSelectedEmployee.length < 1) {
                this.showMultiple = false;
            }
        },

        // Xử lý sự kiện click vào input checkbox nhân viên
        // Author: HMHieu(04-10-22)
        selectEmployee(data) {
            if (!this.listSelectedEmployee.includes(data.employeeID)) {
                this.listSelectedEmployee.push(data.employeeID);

            } else {
                this.listSelectedEmployee = [...this.listSelectedEmployee.filter((obj)=>obj != data.employeeID)];
            }
            this.checkedHeader(this.employee)
            console.log(this.listSelectedEmployee);
        },

        /********************
        * Kiểm tra checkbox ở header table
        * HMHieu 18-11-2022
        * 
        */
        checkedHeader(listTable) {
            let count = 0;
            for (let item of listTable) {
                if (this.listSelectedEmployee.some((obj) => obj == item.employeeID)) {
                    count++;
                }
            }
            if (count == listTable.length) {
                this.isCheckedAll = true;
            }
            else { this.isCheckedAll = false; }
        },

        // Xử lý sự kiện click vào input checkbox tổng
        // Author: HMHieu  04-10-22
        selectAllEmployee(listTable) {
            //nếu chưa check tổng
            if (!this.isCheckedAll) {
                this.isCheckedAll = true;
                let arrayIdInTable = [...listTable.map((obj) => obj.employeeID)];              
            
                this.listSelectedEmployee = [...this.listSelectedEmployee, ...arrayIdInTable];
            } else {//nếu đã check tổng
                this.isCheckedAll = false;
                for (let item of listTable) {
                    this.listSelectedEmployee = [...this.listSelectedEmployee.filter((obj) => obj != item.employeeID)];
                }
            }         
        },

        /**
         * Chức năng xóa nhiều nhân viên
         * CreatedBy: HMHieu (04-10-22)
         */
        async deleteMultiple(listEmplyeeDelete) {

            try {
                const me = this;
                var method = Resource.method.POST;
                var url = urlApi.methodCRUD.deleteMultiple;
                try {
                    var res = await baseApi.fetchAPIBody(listEmplyeeDelete, url, method)
                    this.handleResponseDeleteMul(res, me)
                    this.listSelectedEmployee = [];
                } catch (error) {
                    console.log(error);
                    this.errorBackEnd(eNum.errorBackEnd.Ser);
                }
                  //trả về mảng ds nhân viên rỗng               

            } catch (error) {
                this.openToast(resource.ToastMessage.error);
                this.errorBackEnd(eNum.errorFE.UserActionError);
                console.log(error);
            }
        },

        /**
        * Xử lý dữ liệu trả về của BE khi xóa nhiều
        * @param {*} res response
        * @param {*} me //con trỏ
        * CreatedBy: HMHieu (24-10-2022)
        */
        handleResponseDeleteMul(res, me) {
            let ok = res.success;
            let errorCode = res.errorCode;
            let totalDeleted = res.data;
            if (ok) {
                //this.$emit('openToast', resource.ToastMessage.success);
                this.isShowPopup = false;
                this.isLoadFull = true;
                setTimeout(() => (this.isLoadFull = true), eNum.timeOut.loadData);
                me.openToast(totalDeleted + resource.ToastMessage.successDeleteMultiple);
                // me.$emit("closeDialog");
                me.loadData();
                me.showMultiple = false;
            }
            if (errorCode === eNum.errorBackEnd.InsertFailCode ||
                errorCode === eNum.errorBackEnd.exception) {
                this.errorBackEnd(errorCode)
            }
        },

    }
}
</script>
<style>
/* @import url(../../css/main.css); */
</style>