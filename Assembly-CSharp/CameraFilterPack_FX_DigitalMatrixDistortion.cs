﻿using System;
using UnityEngine;

// Token: 0x0200019F RID: 415
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/FX/DigitalMatrixDistortion")]
public class CameraFilterPack_FX_DigitalMatrixDistortion : MonoBehaviour
{
	// Token: 0x170002BE RID: 702
	// (get) Token: 0x06000ECE RID: 3790 RVA: 0x000715D9 File Offset: 0x0006F7D9
	private Material material
	{
		get
		{
			if (this.SCMaterial == null)
			{
				this.SCMaterial = new Material(this.SCShader);
				this.SCMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
			return this.SCMaterial;
		}
	}

	// Token: 0x06000ECF RID: 3791 RVA: 0x0007160D File Offset: 0x0006F80D
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/FX_DigitalMatrixDistortion");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000ED0 RID: 3792 RVA: 0x00071630 File Offset: 0x0006F830
	private void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
	{
		if (this.SCShader != null)
		{
			this.TimeX += Time.deltaTime;
			if (this.TimeX > 100f)
			{
				this.TimeX = 0f;
			}
			this.material.SetFloat("_TimeX", this.TimeX);
			this.material.SetFloat("_Value", this.Size);
			this.material.SetFloat("_Value2", this.Distortion);
			this.material.SetFloat("_Value5", this.Speed);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000ED1 RID: 3793 RVA: 0x00002ACE File Offset: 0x00000CCE
	private void Update()
	{
	}

	// Token: 0x06000ED2 RID: 3794 RVA: 0x00071712 File Offset: 0x0006F912
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040011A9 RID: 4521
	public Shader SCShader;

	// Token: 0x040011AA RID: 4522
	private float TimeX = 1f;

	// Token: 0x040011AB RID: 4523
	private Material SCMaterial;

	// Token: 0x040011AC RID: 4524
	[Range(0.4f, 5f)]
	public float Size = 1.4f;

	// Token: 0x040011AD RID: 4525
	[Range(-2f, 2f)]
	public float Speed = 0.5f;

	// Token: 0x040011AE RID: 4526
	[Range(-5f, 5f)]
	public float Distortion = 2.3f;
}
