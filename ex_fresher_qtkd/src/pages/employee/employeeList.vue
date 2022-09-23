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
                    <input type="text" class="input fontsize__input" placeholder="Tìm kiếm theo mã, tên nhân viên">
                    <div class="search icon icon__search" ></div>
                </div>
                <div class="content__reload icon icon__reload"></div>
            </div>
            <div class="content__data">
                <table class="table">
                    <thead>
                        <tr>
                            <th class="column__sticky column__checkbox "><input class="input__checkbox" type="checkbox" /> </th>
                            <th>Mã nhân viên</th>
                            <th>Tên nhân viên</th>
                            <th>Giới tính</th>
                            <th>Ngày sinh</th>
                            <th > <div class="tooltip">Số CMND <span class="tooltiptext">Số chứng minh nhân dân</span></div> </th>                            
                            <th>Tên đơn vị</th>
                            <th>Chức danh</th>
                            <th>Số tài khoản</th>
                            <th>Tên ngân hàng</th>
                            <th>Chi nhánh TK ngân hàng</th>
                            <th class=" column__sticky column__edit column__edit--th">Chức năng</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for=" (item, index) in employee" :key="item.id" 
                            @dblclick="showEditForm(item)" >
                            <td class="column__sticky column__checkbox"  @dblclick.stop > <input type="checkbox" class="input__checkbox checkbox__item" /></td >
                            <td class="width__100">{{item.employeeCode}}</td>
                            <td class="width__150">{{item.employeeName}}</td>
                            <td class="width__60">
                                <span v-if="item.gender==0">Nam</span>
                                <span v-if="item.gender==1">Nữ</span>
                                <span v-if="item.gender==2">Khác</span>   
                            </td>
                            <td class="width__100">{{formatDateTable(item.dateOfBirth)}}</td>
                            <td class="width__100">{{item.phoneNumbermobile}}</td>
                            <td class="width__100">{{item.departmentName}}</td>
                            <td class="width__100">{{item.positionName}}</td>
                            <td class="width__100">{{item.accountBank}}</td>
                            <td class="width__150">{{item.nameBank}}</td>
                            <td class="width__200">{{item.branchBank}}</td>
                            
                            <div class="value__edit"  v-show="isShowDropList[index]" :style="{'top': `${topDropList}px`}" >
                                    <div class="edit__item">Nhân bản</div>
                                    <div class="edit__item" @click="showPopup(index,item.id)">Xóa</div>
                                    <!-- @click="btnDelete(item,index)" -->
                                    <div class="edit__item">Ngừng sử dụng</div>
                            </div>    
                            <td class="edit__td column__sticky column__edit width__100"  :ref="'row_'+index" @dblclick.stop  @click="btnShowEdit(index)"> 
                                    Sửa
                                    <div class="icon__drop icon icon__edit--drop" ></div>                                    
                            </td>
                                                  
                        </tr>
                    </tbody>
                </table>
            </div>



            <div class="content__bottom">
                <div class="buttom__left">
                    Tổng số:
                   <span class="total__record">{{totalRecord}}</span>  bản ghi
                </div>
                <div class="buttom__right">
                    <div class="paging__select">
                        <input class="input" type="text">
                        <div class="drop__paging--icon icon icon__paging" ></div>
                        <div class="drop__paging--icon icon icon__paging--up"  hidden></div>
                        <div class="drop__paging drop__paging--item"  hidden>
                            <div class="drop__paging--item ">100 bản ghi trên 1 trang</div>
                            <div class="drop__paging--item">50 bản ghi trên 1 trang</div>
                            <div class="drop__paging--item">30 bản ghi trên 1 trang</div>
                            <div class="drop__paging--item">20 bản ghi trên 1 trang</div>
                            <div class="drop__paging--item">10 bản ghi trên 1 trang</div>
                        </div>
                    </div>
                    <div class="page__number">
                        <div class=" page__number--previous">Trước</div>
                        <div class="page__item page__number--item page__active">1</div>
                        <div class="page__item page__number--item">2</div>
                        <div class="page__item page__number--item">3</div>
                        <div class="page__item page__number--item">...</div>

                        <div class="page__item page__number--item">52</div>
                        <div class=" page__number--next">Sau</div>
                    </div>
                </div>
            </div>

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
        ></emp-detail>        
    </div>
    <MPopup  v-if="isShowPopup" :idEmployee='idEmployee' @employeeDelete='employeeDelete'
    @closePopup='closePopup'  :content="'Bạn có thực sự muốn xóa nhân viên này? '" />
    <MPopupError v-show='isShowPopupError' @closeError='closeError' :text='textError'/>
   
</template>
<script>

import empDetail from './employeeDetail.vue';
import MPopup from '../../components/base/MPopupWaring.vue';
import MPopupError from '../../components/base/MPopupError.vue';
import eNum from '../../js/common/eNum.js';
import common from '@/js/common/common.js';
import resource from '../../js/common/resource.js'

    /**
     * URL api 
     */
     var URL =process.env.VUE_APP_URL;

export default {

    name: "employeeList",
    components: { empDetail,MPopup,MPopupError},
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
            totalRecord:Number,
            isShowPopup:false,
            idEmployee: '',
            isShowPopupError:false,
            textError: '',
          
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
            fetch(URL, { method: "GET" })
                .then((res) => res.json())
                .then((data) => {
                    this.employee = data;
                    this.totalRecord=this.employee.length;                           
                })
                .catch((res) => {
                    console.log(res);            
                });
        },
        /**
         * Đóng form chức năng xóa nhân viên 
         * Author: HMHieu(19/09/22)
         */
        showPopup(index,id){
            this.isShowPopup = true;
            this.idEmployee=id;
            console.log(id);
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
                    default:
                        break;
                }
            } catch (error) {
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
         * Xóa nhân viên theo ID
         * Author: HMHieu(19/09/22)
         */
        employeeDelete(idEmployee){          
            // var me = this;
            var method = "Delete";             
            var DeleteURL= URL+`/${idEmployee}`;                 
            // Xóa dữ liệu:            
            fetch(DeleteURL, {                
                method: method,
                headers: {
                "Content-Type": "application/json",
                },                
            })
                .then((res) => res.json())
                .then((res) => {               
                     this.isShowPopup = false;                 
                    this.loadData();
                    console.log(res);
                })
                .catch((res) => {
                    console.log(res);
                
                });
        },            
    }
}
</script>
<style>

</style>