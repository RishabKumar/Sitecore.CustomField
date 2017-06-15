; (function () {
	'use strict';
	
	// splittingHours();
	var interval;
	 
	$.ajax({
	                url: "api/ShowsData/GetCurrentWeekShows?today=29/05/2017",
                    //force to handle it as text
                    //dataType: "json",
                    success: function(data) {
                        var json = $.parseJSON(data);
                        var saveTemplate, currentDay;
                        console.log(json.length);
			var template='<div class="block"><div class="innerblock"><span class="broadcast-date">{broadcastDate}</span><span class="date">{date}</span><span class="time">{time}</span><span class="duration">{duration}</span><span class="material-id">{MaterialID}</span><span class="title">{title}</span><span class="episode-name">{episodeName}</span><span class="description">{description}</span><span class="title-id">{titleId}</span><span class="episode-id">{episodeId}</span><span class="plan-title-id">{planTitleId}</span><span class="day-name">{dayName}</span><span class="show-definition">{showDefinition}</span></div></div>';
			for(var i=0; i<json.length; i++){
			
				saveTemplate= template.replace('{broadcastDate}',json[i].BroadcastDate).replace('{date}',json[i].PlanDate).replace('{time}',json[i].PlanTime).replace('{duration}',json[i].PlanDuration).replace('{MaterialID}',json[i].MaterialID).replace('{title}',json[i].TitleName).replace('{episodeName}',json[i].EpisodeName).replace('{description}',json[i].EPG).replace('{titleId}',json[i].TitleID).replace('{episodeId}',json[i].EpisodeID).replace('{planTitleId}',json[i].PlanTitleID).replace('{dayName}',json[i].DayName).replace('{showDefinition}',json[i].Definition);
				$('.program-list.slider-show-list .inner-slider-show-list.program-info-list').append(saveTemplate);
			}
			//seekbarPosition();
			//currentShow();
			interval = setInterval(function(){
				hideArrows();			
				currentDay = getCurrentDay();
				seekbarPosition();
				currentShow(currentDay);
				setTimeout(function(){ 
					positionShow();
				}, 1000);
			}, 1000);
                    }
                });
	var transformVal=0;
	
	
	function hideArrows(){
		if($('.week-info-list li:first-child').hasClass('active')){
			$('.left-btn').hide();
		}else{
			$('.left-btn').show();
		}	
		if($('.week-info-list li:last-child').hasClass('active')){
			$('.right-btn').hide();
		}else{
			$('.right-btn').show();
		}
	}
	
	$('.week-info-list li').click(function(){
		var currentDay = $(this).text();
		$('.progress-line').hide();
		clearInterval(interval);
		currentShow(currentDay);
		slideOnListClick(currentDay);
		hideArrows();
	});
	
	$('button.right-btn').click(function(){
		// clearInterval(interval);
		// transformVal=parseFloat($('.inner-slider-show-list').css('transform').split(/[()]/)[1].split(',')[4]) - $('.slider-show-list').width();
		// console.log(transformVal);
		// $('.inner-slider-show-list').css({"transition-duration":"500ms","transition-timing-function":"cubic-bezier(0.12, 0.52, 0.1, 0.91)"});
		// if(transformVal<0){
			// $('.inner-slider-show-list').css("transform","translateX(" + (transformVal) + "px)");
		// }
		var index = $('.week-info-list li.active').index();
		if(index <= 7){
			slideOnListClick($('.week-info-list li.active').text())
		}
	});
	$('button.left-btn').click(function(){	
		// clearInterval(interval);
		// //var blockWidth= totalWidth() - ($('.slider-show-list').width());
		// transformVal=parseFloat($('.inner-slider-show-list').css('transform').split(/[()]/)[1].split(',')[4]) + $('.slider-show-list').width();
		// $('.inner-slider-show-list').css({"transition-duration":"500ms","transition-timing-function":"cubic-bezier(0.12, 0.52, 0.1, 0.91)"});
		// console.log(transformVal);
		// if((-transformVal) < totalWidth()){
			// $('.inner-slider-show-list').css("transform","translateX(" + (transformVal) + "px)");
		// }
		var index = $('.week-info-list li.active').index();
		if(index >= 0){
			slideOnListClick($('.week-info-list li.active').text())
		}
		
	});
	
	function slideOnListClick(day){
		for(var i=0; i<$('.block span.day-name').length; i++){
			if($('.block span.day-name').eq(i).text()===day){
				var boxWidth=$('.block span.day-name').eq(i).parent().parent().width();
				console.log($('.block span.title-id').eq(i));
				var duration= splitDurationTime($('.block span.duration').eq(i).text());
				var startTime= splitTime($('.block span.time').eq(i).text());
				var move= showTimeBox(boxWidth,duration,startTime);
				$('.inner-slider-show-list').css({"transition-duration":"500ms","transition-timing-function":"cubic-bezier(0.12, 0.52, 0.1, 0.91)"});
				$('.inner-slider-show-list').css("transform","translateX(" + (-transformX(i,move)) + "px)");
				break;
			}
		}
	}
	/*Position of show*/
	function positionShow(){
		var boxWidth=$('.block').width();
		var transformVal=parseFloat($('.inner-slider-show-list').css('transform').split(/[()]/)[1].split(',')[4]);
		
		$('.block').each(function() {    
		   if($(this).hasClass('active')){
			boxWidth=$('.block.active').width();
			//console.log($('.block.active span.duration').text());
			var duration= splitDurationTime($('.block.active span.duration').text());
			var startTime= splitTime($('.block.active span.time').text());
			var indexVal=$(this).index();
			var move= showTimeBox(boxWidth,duration,startTime);
			$('.inner-slider-show-list').css({"transition-duration":"500ms","transition-timing-function":"cubic-bezier(0.12, 0.52, 0.1, 0.91)"});
			$('.inner-slider-show-list').css("transform","translateX(" + (-transformX(indexVal,move)) + "px)");	
		   }
		});
		
		
	}
	
	/*Showtime Box movement*/
	function showTimeBox(activeBoxWidth,activeDuration,activeStartTime){
		var curTime=getCurrentTime();
		var remDuration = ((curTime-activeStartTime)/activeDuration);
		var widthMovePixel = remDuration*activeBoxWidth;
		return widthMovePixel;
	}
	
	/*Get Current position of show*/
	function transformX(index,moveWidth){
		var temp=0;		
		var fullDayTime = ((23*60*60) + (59*60) + 60);
		var seekPosition=seekbarPosition();
		var t = (seekPosition/100)*($('.epg-player-info').width());
		for(var g=0; g<index;g++){ 
			temp= temp + parseInt($('.block').eq(g).width());
		}
		//console.log(moveWidth);
		temp = temp - t + index*8 + moveWidth;
		//console.log(Math.floor(temp));
		return Math.floor(temp);
	}
	
	/*Get current Time*/
	function getCurrentTime(){
		var d = new Date();
		var time = ((d.getHours()*60*60) + (d.getMinutes()*60) + d.getSeconds());
		return time;
	}
	
	/*Get current Time*/
	function getCurrentDay(){
		var d = new Date();
		var day ='';
		var dayNumber = d.getDay();
		switch (dayNumber) {
		    case 0:
			day = "Sunday";
			break;
		    case 1:
			day = "Monday";
			break;
		    case 2:
			day = "Tuesday";
			break;
		    case 3:
			day = "Wednesday";
			break;
		    case 4:
			day = "Thursday";
			break;
		    case 5:
			day = "Friday";
			break;
		    case  6:
			day = "Saturday";
		}
		return day;
	}
	
	/*Split time in seconds for duration*/
	function splitDurationTime(a){
		var splitTimeSec=0;
		var splitTime=a.split(':');//console.log(a.length);
		if(splitTime.length === 2){
			splitTimeSec=(parseInt(splitTime[0]*60)+parseInt(splitTime[1]));
		}
		if(splitTime.length === 3){
			splitTimeSec=(parseInt(splitTime[0]*60*60)+parseInt(splitTime[1]*60)+parseInt(splitTime[2]));
		}
		
		return splitTimeSec;
	}
	
	/*Split time in seconds*/
	function splitTime(a){
		var splitTimeSec=0;
		var splitTime=a.split(':');//console.log(a.length);
		if(splitTime.length === 2){
			splitTimeSec=(parseInt(splitTime[0]*60*60)+parseInt(splitTime[1]*60));
		}
		if(splitTime.length === 3){
			splitTimeSec=(parseInt(splitTime[0]*60*60)+parseInt(splitTime[1]*60)+parseInt(splitTime[2]));
		}
		
		return splitTimeSec;
	}
	
	/*Get current seekbar position as per current time*/
	function seekbarPosition(){
		var currentTime = getCurrentTime();
		var fullDayTime = ((23*60*60) + (59*60) + 60);
		var seekbarwidth= (currentTime/fullDayTime)*100;
		$('.progress-line').css({ left: seekbarwidth+'%'}, 'slow');
		return seekbarwidth;
	}
	
	/*Get total width of shows*/
	function totalWidth(){
		var temp=0;
		
		for(var g=0; g<$('.block').length;g++){ 
			temp= temp + parseInt($('.block').eq(g).width());
			if(g===$('.block').length-1){
				temp=temp+(parseInt($('.block').eq(g).width())/2)
			}
		}
		
		return temp;
	}
	
	function lengthCheck(a){
		var time2 =0;
		if(a.length==2){
			time2 = (parseInt(a[0]*60*60)+parseInt(a[1]*60));
		}
		if(a.length==3){
			time2 = (parseInt(a[0]*60*60)+parseInt(a[1]*60)+parseInt(a[2]));
		}
		return time2;
	}
	
	function durationCheck(a){
		var time2 =0;
		if(a.length==2){
			time2 = (parseInt(a[0]*60)+parseInt(a[1]));
		}
		if(a.length==3){
			time2 = (parseInt(a[0]*60*60)+parseInt(a[1]*60)+parseInt(a[2]));
		}
		return time2;
	}
	
	/*Get current show*/
	function currentShow(currentDay){
		var z,z3,z2,a,count,count2,time2,time3,duration;
		var dur=[];
		dur=$('span.duration');
		var dayname=$('span.day-name');
		var lidata=$('.week-info-list li');
		var arr=[];
		arr = $('span.time');
		count = arr.length;
		count2 = lidata.length;
		var currentTime = getCurrentTime();
		var today=getCurrentDay();
		for(var i = 0; i < count; i++){
			z= arr.eq(i).text();			
			a= z.split(':');
			time2= lengthCheck(a);
			var p =dur.eq(i).text();
			var q=p.split(':');
			duration = durationCheck(q);
			arr.eq(i).parent().parent().css('width',(duration/20)+'px');
			time3= time2+duration;
			for(var j = 0; j < count2; j++){
				z2=dayname.eq(i).text();
				z3=lidata.eq(j).text();
				if((time3>currentTime) && (currentTime>time2) && currentDay==z2){
					if(today === currentDay){
						arr.eq(i).parent().parent().addClass('active');	
					}					
					if(z3===z2){
						lidata.eq(j).addClass('active');
					}else{
						lidata.eq(j).removeClass('active');
					}
					
				}else{
					arr.eq(i).parent().parent().removeClass('active');
				}
			}
		}
	}
	
	
}());