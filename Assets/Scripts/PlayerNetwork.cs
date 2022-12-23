using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class PlayerNetwork : NetworkBehaviour
{
    private float x;
    private float y;
    //[SerializeField] private PlayerMovement pm;
    public Animator anim;


    // Update is called once per frame
    [Client]
    private void Update()
    {
        if(!hasAuthority) { return; }
        //CmdMove();
        if (Math.Abs(Input.GetAxisRaw("Horizontal")) >= 0.01 || Math.Abs(Input.GetAxisRaw("Vertical")) >= 0.01)
        {
            CmdMove();
        }

        
        
    }
    [Command]
    private void CmdMove()
    {
        //Validate Logic

        RpcMove();
    }
    [ClientRpc]
    private void RpcMove()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        //Debug.Log(y);



    }


}
