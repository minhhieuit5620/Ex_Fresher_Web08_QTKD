<template>
      <div class="sidebar">
            <router-link to="/employeeList" class="sidebar__item">
                <div class="sidebar__item--icon icon__sidebar icon icon__dashboard" ></div>
                <div class="sidebar__item--text"> Tổng quan</div>
            </router-link>
            <router-link to="/moneyPage" class="sidebar__item">
                <div class="sidebar__item--icon icon__sidebar icon icon__cash" ></div>
                <div class="sidebar__item--text"> Tiền mặt</div>
            </router-link>
            <router-link to="/moneyBank" class="sidebar__item">
                <div class="sidebar__item--icon icon__sidebar icon icon__bank" ></div>
                <div class="sidebar__item--text"> Tiền gửi</div>
            </router-link>
            <router-link to="/salePage" class="sidebar__item">
                <div class="sidebar__item--icon icon__sidebar icon icon__buy" ></div>
                <div class="sidebar__item--text"> Mua hàng</div>
            </router-link>
            <router-link to="/buy" class="sidebar__item">
                <div class="sidebar__item--icon icon__sidebar icon icon__shop" ></div>
                <div class="sidebar__item--text"> Bán hàng</div>
            </router-link>
            <router-link to="/orderPage" class="sidebar__item">
                <div class="sidebar__item--icon icon__sidebar icon icon__Mana--order" ></div>
                <div class="sidebar__item--text"> Quản lý hóa đơn</div>
            </router-link>
            <router-link to="/warePage" class="sidebar__item">
                <div class="sidebar__item--icon icon__sidebar icon icon__warehouse" ></div>
                <div class="sidebar__item--text"> Kho</div>
            </router-link>
            <router-link to="/ccdcPage" class="sidebar__item">
                <div class="sidebar__item--icon icon__sidebar icon icon__ccdc" ></div>
                <div class="sidebar__item--text"> Công cụ dụng cụ</div>
            </router-link>
            <router-link to="/assets" class="sidebar__item">
                <div class="sidebar__item--icon icon__sidebar icon icon__tscd" ></div>
                <div class="sidebar__item--text"> Tài sản cố định</div>
            </router-link>
            <router-link to="/taxPage" class="sidebar__item">
                <div class="sidebar__item--icon icon__sidebar icon icon__tax" ></div>
                <div class="sidebar__item--text"> Thuế</div>
            </router-link>
            <router-link to="/taxPage" class="sidebar__item">
                <div class="sidebar__item--icon icon__sidebar icon icon__price" ></div>
                <div class="sidebar__item--text"> Giá thành</div>
            </router-link>            
            <router-link to="/taxPage" class="sidebar__item">
                <div class="sidebar__item--icon icon__sidebar icon icon__total" ></div>
                <div class="sidebar__item--text"> Tổng hợp</div>
            </router-link>
            <router-link to="/taxPage" class="sidebar__item">
                <div class="sidebar__item--icon icon__sidebar icon icon__budget" ></div>
                <div class="sidebar__item--text"> Ngân sách</div>
            </router-link>
            <router-link to="/taxPage" class="sidebar__item">
                <div class="sidebar__item--icon icon__sidebar icon icon__report" ></div>
                <div class="sidebar__item--text"> Báo cáo</div>
            </router-link>
            <router-link to="/taxPage" class="sidebar__item" >
                <div class="sidebar__item--icon icon__sidebar icon icon__pttc" ></div>
                <div class="sidebar__item--text"> Phân tích tài chính</div>
            </router-link>
        </div>
</template>
<script>



export default {
    data() {
        return{}
    },
}
</script>