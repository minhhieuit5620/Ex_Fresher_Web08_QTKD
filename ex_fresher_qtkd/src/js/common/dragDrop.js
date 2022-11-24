/* eslint-disable */
// export default function drag(){
  /* eslint-disable */
//USAGE  : just add "draggable" prop to any element
//EXAMPLE: <div draggable> some stuff </div>
// "use strict";
export const drag = () => {

  [].forEach.call(document.querySelectorAll("[draggable]"), function (item) {
    var tgtRect = item.getBoundingClientRect();
    (item.move = false),
      ((item.startPos = {
        X: 0,
        Y: 0,
      }),
      (item.initPos = {
        X: tgtRect.left,
        Y: tgtRect.top,
      })),      
      item.addEventListener("mousedown", function (e) {
        if (e.path[0].tagName === "INPUT" || e.path[0].tagName === "BUTTON")
          return;
        var current = this;
        var startMousePos = {
          X: e.clientX,
          Y: e.clientY,
        };
        current.move = true;
        current.style.zIndex = setHighestZIndex();
        current.classList.add("draggable--moving");
        window.addEventListener("mouseup", function (e) {
          item.style.cursor = "default";
          current.move = false;
          var positionString = current.style.transform.replace(
            /[^\d\,\-]*/g,
            ""
          );
          current.startPos.X = +positionString.split(",")[4];
          current.startPos.Y = +positionString.split(",")[5];
          current.classList.remove("draggable--moving");
        });

        function setHighestZIndex() {
          var highest = 0;
          [].forEach.call(
            document.querySelectorAll("[draggable]"),
            function (item) {
              var zindex = getComputedStyle(item, null).getPropertyValue(
                "z-index"
              );
              if (zindex > highest && zindex != "auto") {
                highest = zindex;
              }
            }
          );
          return +highest + 1;
        }
        window.addEventListener("mousemove", function (e) {
          if (current.move) {
            item.style.cursor = "move";
            current.style.transform =
              "matrix(1, 0, 0, 1, " +
              (e.clientX - startMousePos.X + current.startPos.X) +
              ", " +
              (e.clientY - startMousePos.Y + current.startPos.Y) +
              ")";
          }
        });
      });
  });
};
//-------------------------------------------------------
    // const main = document.getElementById('range');
    // const icon = document.querySelector('.dialog__infoEmp');
    // console.log(main,icon);
    // let move = false;
    // let deltaLeft = 0, deltaTop = 0;
    
    // icon.addEventListener('mousedown', function (e) {
    
    //   deltaLeft = e.clientX-e.target.offsetLeft;
    //   deltaTop = e.clientY-e.target.offsetTop;
    //   move = true;
    //   icon.setAttribute('style','cursor: grab;')
    // })
    
    // icon.addEventListener('mousemove', function (e) {
    //   if (move) {
    //     const cx = e.clientX;
    //     const cy = e.clientY;
      
    //     let dx = cx - deltaLeft
    //     let dy = cy - deltaTop
    
      
    //     console.log(dx,dy);//xét tràn form ra ngoài
    //     if (dx < -322) dx = -322//tràn trái
    //     if (dy < -80) dy = -80//tràn trên
    //     if (dx > 322) dx = 322//tràn phải
    //     if (dy > 80) dy = 80//tràn dưới
    //     // if (dx > screen.width) dx = e.clientX
    //     // if (dy > screen.height) dy = e.clientY
    //     icon.setAttribute('style', `transform:translate(${dx}px, ${dy}px)`)
     
     
    //   }
    // })
    
    // main.addEventListener('mouseup', function (e) {
    //   move = false;
    //   console.log('mouseup');
    // })
   //-------------------------------------------------------------
    // var box = document.querySelector(".dialog__infoEmp");
    // var container = document.querySelector(".dialog__wrap");
    
    // // var box = document.querySelector(".box");
    // // var container = document.querySelector(".container");
    
    // var clicked = false;  //flag used to determine whether div is clicked or not
    
    
    // //mouse events
    // box.addEventListener("mousedown",function(e){
         
    //     clicked = true ;
    // });
    
    // box.addEventListener("mouseup",function(e){
         
    //     clicked = false;
    // });
    
    // container.addEventListener("mouseleave",function(e){
         
    //     clicked = false;
    // });
    
    // container.addEventListener("mousemove",function(e){
    //   e.preventDefault();
    //   if(e.target == container ){ //prevent mousemove event from triggering on box 
    //       if(clicked){
    //         box.style.transform = "translate(" + (e.offsetX - 25 ) + "px" + "," + (e.offsetY - 25) + "px" + ")"; 
    //       }
    //   }
    // });
    
    // //touch events
    // box.addEventListener("touchstart",function(e){
         
    //     clicked = true ;
    // });
    
    // box.addEventListener("touchend",function(e){
         
    //     clicked = false;
    // });
    
    // container.addEventListener("touchmove",function(e){
    //   e.preventDefault();
    //   if(clicked){
    //         box.style.transform = "translate(" + (e.touches[0].clientX - container.offsetLeft ) + "px" + "," + (e.touches[0].clientY - container.offsetTop ) + "px" + ")"; 
    //       }
    // });
    
    
// }