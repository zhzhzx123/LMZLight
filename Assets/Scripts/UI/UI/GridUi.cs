using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

//1.控制物品的拖动
//2.显示物品

public class GridUi : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {

    private Image image;//显示物品的组件，拖动的也是它
    public string imageName;//物体图片的名字，如果名字不为空，有物体有“”
    public Transform tempParent;//临时的父节点，解决其他限制图片问题
    //public GameObject Scene;
    private RectTransform imagePrt;
    private Vector2 outPos;
    private Vector2 offset;//鼠标开始拖动时距离图片中心点的偏移量

    public GameObject Locked;
    public GameObject GunIcon1;
    // public GameObject Lock;
    private void Awake()
    {
        image = transform.Find("GunIcon").GetComponent<Image>();
        image.raycastTarget = false;
        imagePrt = tempParent as RectTransform;



    }
    private void Start()
    {
        UpdateItem();
    }
    void UpdateItem()
    {
        if (imageName =="")
        {
            image.enabled = false;
        }
        else
        {
            image.enabled = true;

            if (image.sprite.name !=imageName)
            {

            Sprite sp = Resources.Load<Sprite>("UI/WeaponIcon/" + imageName);
            image.sprite = sp;
            }
        }

    }

    //设置自己的图片名字,返回值是之前的名字
    public string SetSpriteName(string str)
    {
        string before = imageName;
        imageName = str;
        UpdateItem();
        return before;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (imageName =="")
        {
            return;
        }
       // Debug.Log("使用");
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle
             (
               imagePrt, eventData.position,
               eventData.enterEventCamera,
               out outPos
             ))
        {
           
            image.transform.localPosition = outPos - offset;
        }

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (imageName == "")
        {
            return;
        }
        //开始时，把图片放在临时的父节点上，保证拖动的图片永远显示在最前面
        image.transform.parent = tempParent;
       
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle
           (
             imagePrt, eventData.position,
             eventData.enterEventCamera,
             out outPos
           ))
        {
            //计算鼠标距离图片中心点的偏移量
            offset= outPos - new Vector2(image.transform.localPosition.x, image.transform.localPosition.y);
//            Lock.SetActive(true);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (imageName == "")
        {
            return;
        }
        //结束时，需要把图片的父节点还原，位置也需要归零
        image.transform.parent = transform;
        image.transform.localPosition = Vector3.zero;
      //  Lock.SetActive(false);


        //拖动结束时，判断当前的鼠标位置是否在格子上
        if (eventData.pointerCurrentRaycast.gameObject != null 
            && eventData.pointerCurrentRaycast.gameObject.CompareTag("Gird"))
        {//通过比较检测物体的标签判断是否是格子

            //把自己的图片给对方，把对方的隐藏
            GridUi target = eventData.pointerCurrentRaycast.gameObject.GetComponent<GridUi>();
            //把自己的图片名字给目标，temp就是对方的图片名字
            string temp = target.SetSpriteName(this.imageName);



            //然后把对方的图片名字设置给自己
            // this.SetSpriteName(temp);
          
        }
        else
        {
           
        }
    }

    public void Ontransmission()
    {
        //OnEndDrag();
    }

}
