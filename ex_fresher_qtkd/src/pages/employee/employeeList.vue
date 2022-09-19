<template>

    <div class="content">
        <div class="top__content">
            <div class="top__content--title fontsize__tile">
                Nhân viên
            </div>
            <div class="top__content--add">
                <button class="btn add__emp" @click="addOnClick"> Thêm mới nhân viên</button>
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
                            <th class="tooltip">Số CMND <span class="tooltiptext">Số chứng minh nhân dân</span></th>                            
                            <th>Tên đơn vị</th>
                            <th>Chức danh</th>
                            <th>Số tài khoản</th>
                            <th>Tên ngân hàng</th>
                            <th>Chi nhánh TK ngân hàng</th>
                            <th class=" column__sticky column__edit column__edit--th">Chức năng</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for=" (item, index) in employee" :key="item.EmployeeId" 
                            @dblclick="showEditForm(item)" >
                            <td class="column__sticky column__checkbox"  @dblclick.stop > <input type="checkbox" class="input__checkbox checkbox__item" /></td >
                            <td style="width: 100px">{{item.employeeCode}}</td>
                            <td style="width: 150px">{{item.employeeName}}</td>
                            <td style="width: 60px">{{item.gender}}</td>
                            <td style="width: 100px">{{item.dateOfBirth}}</td>
                            <td style="width: 100px">{{item.phoneNumbermobile}}</td>
                            <td style="width: 100px">{{item.departmentName}}</td>
                            <td style="width: 100px">{{item.positionName}}</td>
                            <td style="width: 100px">{{item.accountBank}}</td>
                            <td style="width: 150px">{{item.nameBank}}</td>
                            <td style="width: 200px">{{item.branchBank}}</td>
                            
                            <div class="value__edit"  v-show="isShowDropList[index]" :style="{'top': `${topDropList}px`}" >
                                    <div class="edit__item">Nhân bản</div>
                                    <div class="edit__item">Xóa</div>
                                    <div class="edit__item">Ngừng sử dụng</div>
                            </div>    
                            <td class="edit__td column__sticky column__edit" style="width: 100px" :ref="'row_'+index" @dblclick.stop> 
                                    Sửa
                                    <div class="icon__drop icon icon__edit--drop"  @click="btnEdit(index)"></div>                                    
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
        @closeDialog="closeDialog"></emp-detail>        
    </div>
</template>
<script>

import empDetail from './employeeDetail.vue'
import eNum from '../../js/common/eNum'
export default {

    name: "employeeList",
    components: { empDetail },
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
        btnEdit(index) {                  
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
         * Lấy toàn bộ nhân viên và tổng số bản ghi hiện có
         * Author: HMHieu(18/09/22)
         */
        loadData() {
            fetch("http://localhost:3000/employee", { method: "GET" })
                .then((res) => res.json())
                .then((data) => {
                    this.employee = data;
                    this.totalRecord=this.employee.length;                           
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