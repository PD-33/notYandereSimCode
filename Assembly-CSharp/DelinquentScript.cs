﻿using System;
using UnityEngine;

// Token: 0x0200025F RID: 607
public class DelinquentScript : MonoBehaviour
{
	// Token: 0x06001327 RID: 4903 RVA: 0x0009F598 File Offset: 0x0009D798
	private void Start()
	{
		this.EasterHair.SetActive(false);
		this.Bandanas.SetActive(false);
		this.OriginalRotation = base.transform.rotation;
		this.LookAtTarget = this.Default.position;
		if (this.Weapon != null)
		{
			this.Weapon.localPosition = new Vector3(this.Weapon.localPosition.x, -0.145f, this.Weapon.localPosition.z);
			this.Rotation = 90f;
			this.Weapon.localEulerAngles = new Vector3(this.Rotation, this.Weapon.localEulerAngles.y, this.Weapon.localEulerAngles.z);
		}
	}

	// Token: 0x06001328 RID: 4904 RVA: 0x0009F664 File Offset: 0x0009D864
	private void Update()
	{
		this.DistanceToPlayer = Vector3.Distance(base.transform.position, this.Yandere.transform.position);
		AudioSource component = base.GetComponent<AudioSource>();
		if (this.DistanceToPlayer < 7f)
		{
			this.Planes = GeometryUtility.CalculateFrustumPlanes(this.Eyes);
			if (GeometryUtility.TestPlanesAABB(this.Planes, this.Yandere.GetComponent<Collider>().bounds))
			{
				RaycastHit raycastHit;
				if (Physics.Linecast(this.Eyes.transform.position, this.Yandere.transform.position + Vector3.up, out raycastHit))
				{
					if (raycastHit.collider.gameObject == this.Yandere.gameObject)
					{
						this.LookAtPlayer = true;
						if (this.Yandere.Armed)
						{
							if (!this.Threatening)
							{
								component.clip = this.SurpriseClips[UnityEngine.Random.Range(0, this.SurpriseClips.Length)];
								component.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
								component.Play();
							}
							this.Threatening = true;
							if (this.Cooldown)
							{
								this.Cooldown = false;
								this.Timer = 0f;
							}
						}
						else
						{
							if (this.Yandere.CorpseWarning)
							{
								if (!this.Threatening)
								{
									component.clip = this.SurpriseClips[UnityEngine.Random.Range(0, this.SurpriseClips.Length)];
									component.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
									component.Play();
								}
								this.Threatening = true;
								if (!this.Yandere.Chased && this.Yandere.Chasers == 0)
								{
									this.DelinquentManager.Attacker = this;
									this.Run = true;
								}
								this.Yandere.Chased = true;
							}
							else if (!this.Threatening && this.DelinquentManager.SpeechTimer == 0f)
							{
								component.clip = ((this.Yandere.Container == null) ? this.ProximityClips[UnityEngine.Random.Range(0, this.ProximityClips.Length)] : this.CaseClips[UnityEngine.Random.Range(0, this.CaseClips.Length)]);
								component.Play();
								this.DelinquentManager.SpeechTimer = 10f;
							}
							this.LookAtPlayer = true;
						}
					}
					else
					{
						this.LookAtPlayer = false;
					}
				}
			}
			else
			{
				this.LookAtPlayer = false;
			}
		}
		if (!this.Threatening)
		{
			if (this.Shoving)
			{
				this.targetRotation = Quaternion.LookRotation(this.Yandere.transform.position - base.transform.position);
				base.transform.rotation = Quaternion.Slerp(base.transform.rotation, this.targetRotation, 10f * Time.deltaTime);
				this.targetRotation = Quaternion.LookRotation(base.transform.position - this.Yandere.transform.position);
				this.Yandere.transform.rotation = Quaternion.Slerp(this.Yandere.transform.rotation, this.targetRotation, 10f * Time.deltaTime);
				if (this.Character.GetComponent<Animation>()[this.ShoveAnim].time >= this.Character.GetComponent<Animation>()[this.ShoveAnim].length)
				{
					this.LookAtTarget = this.Neck.position + this.Neck.forward;
					this.Character.GetComponent<Animation>().CrossFade(this.IdleAnim, 1f);
					this.Shoving = false;
				}
				if (this.Weapon != null)
				{
					this.Weapon.localPosition = new Vector3(this.Weapon.localPosition.x, Mathf.Lerp(this.Weapon.localPosition.y, 0f, Time.deltaTime * 10f), this.Weapon.localPosition.z);
					this.Rotation = Mathf.Lerp(this.Rotation, 0f, Time.deltaTime * 10f);
					this.Weapon.localEulerAngles = new Vector3(this.Rotation, this.Weapon.localEulerAngles.y, this.Weapon.localEulerAngles.z);
				}
			}
			else
			{
				this.Shove();
				base.transform.rotation = Quaternion.Slerp(base.transform.rotation, this.OriginalRotation, Time.deltaTime);
				if (this.Weapon != null)
				{
					this.Weapon.localPosition = new Vector3(this.Weapon.localPosition.x, Mathf.Lerp(this.Weapon.localPosition.y, -0.145f, Time.deltaTime * 10f), this.Weapon.localPosition.z);
					this.Rotation = Mathf.Lerp(this.Rotation, 90f, Time.deltaTime * 10f);
					this.Weapon.localEulerAngles = new Vector3(this.Rotation, this.Weapon.localEulerAngles.y, this.Weapon.localEulerAngles.z);
				}
			}
		}
		else
		{
			this.targetRotation = Quaternion.LookRotation(this.Yandere.transform.position - base.transform.position);
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, this.targetRotation, 10f * Time.deltaTime);
			if (this.Weapon != null)
			{
				this.Weapon.localPosition = new Vector3(this.Weapon.localPosition.x, Mathf.Lerp(this.Weapon.localPosition.y, 0f, Time.deltaTime * 10f), this.Weapon.localPosition.z);
				this.Rotation = Mathf.Lerp(this.Rotation, 0f, Time.deltaTime * 10f);
				this.Weapon.localEulerAngles = new Vector3(this.Rotation, this.Weapon.localEulerAngles.y, this.Weapon.localEulerAngles.z);
			}
			if (this.DistanceToPlayer < 1f)
			{
				if (this.Yandere.Armed || this.Run)
				{
					if (!this.Yandere.Attacked)
					{
						if (this.Yandere.CanMove && ((!this.Yandere.Chased && this.Yandere.Chasers == 0) || (this.Yandere.Chased && this.DelinquentManager.Attacker == this)))
						{
							AudioSource component2 = this.DelinquentManager.GetComponent<AudioSource>();
							if (!component2.isPlaying)
							{
								component2.clip = this.AttackClip;
								component2.Play();
								this.DelinquentManager.enabled = false;
							}
							if (this.Yandere.Laughing)
							{
								this.Yandere.StopLaughing();
							}
							if (this.Yandere.Aiming)
							{
								this.Yandere.StopAiming();
							}
							this.Character.GetComponent<Animation>().CrossFade(this.SwingAnim);
							this.MyWeapon.SetActive(true);
							this.Attacking = true;
							this.Yandere.Character.GetComponent<Animation>().CrossFade("f02_swingB_00");
							this.Yandere.RPGCamera.enabled = false;
							this.Yandere.CanMove = false;
							this.Yandere.Attacked = true;
							this.Yandere.EmptyHands();
						}
					}
					else if (this.Attacking)
					{
						if (this.AudioPhase == 1)
						{
							if (this.Character.GetComponent<Animation>()[this.SwingAnim].time >= this.Character.GetComponent<Animation>()[this.SwingAnim].length * 0.3f)
							{
								this.Jukebox.SetActive(false);
								this.AudioPhase++;
								component.pitch = 1f;
								component.clip = this.Strike;
								component.Play();
							}
						}
						else if (this.AudioPhase == 2 && this.Character.GetComponent<Animation>()[this.SwingAnim].time >= this.Character.GetComponent<Animation>()[this.SwingAnim].length * 0.85f)
						{
							this.AudioPhase++;
							component.pitch = 1f;
							component.clip = this.Crumple;
							component.Play();
						}
						this.targetRotation = Quaternion.LookRotation(base.transform.position - this.Yandere.transform.position);
						this.Yandere.transform.rotation = Quaternion.Slerp(this.Yandere.transform.rotation, this.targetRotation, 10f * Time.deltaTime);
					}
				}
				else
				{
					this.Shove();
				}
			}
			else if (!this.ExpressedSurprise)
			{
				this.Character.GetComponent<Animation>().CrossFade(this.SurpriseAnim);
				if (this.Character.GetComponent<Animation>()[this.SurpriseAnim].time >= this.Character.GetComponent<Animation>()[this.SurpriseAnim].length)
				{
					this.ExpressedSurprise = true;
				}
			}
			else if (this.Run)
			{
				if (this.DistanceToPlayer > 1f)
				{
					base.transform.position = Vector3.MoveTowards(base.transform.position, this.Yandere.transform.position, Time.deltaTime * this.RunSpeed);
					this.Character.GetComponent<Animation>().CrossFade(this.RunAnim);
					this.RunSpeed += Time.deltaTime;
				}
			}
			else if (!this.Cooldown)
			{
				this.Character.GetComponent<Animation>().CrossFade(this.ThreatenAnim);
				if (!this.Yandere.Armed)
				{
					this.Timer += Time.deltaTime;
					if (this.Timer > 2.5f)
					{
						this.Cooldown = true;
						if (!this.DelinquentManager.GetComponent<AudioSource>().isPlaying)
						{
							this.DelinquentManager.SpeechTimer = Time.deltaTime;
						}
					}
				}
				else
				{
					this.Timer = 0f;
					if (this.DelinquentManager.SpeechTimer == 0f)
					{
						this.DelinquentManager.GetComponent<AudioSource>().clip = this.ThreatenClips[UnityEngine.Random.Range(0, this.ThreatenClips.Length)];
						this.DelinquentManager.GetComponent<AudioSource>().Play();
						this.DelinquentManager.SpeechTimer = 10f;
					}
				}
			}
			else
			{
				if (this.DelinquentManager.SpeechTimer == 0f)
				{
					AudioSource component3 = this.DelinquentManager.GetComponent<AudioSource>();
					if (!component3.isPlaying)
					{
						component3.clip = this.SurrenderClips[UnityEngine.Random.Range(0, this.SurrenderClips.Length)];
						component3.Play();
						this.DelinquentManager.SpeechTimer = 5f;
					}
				}
				this.Character.GetComponent<Animation>().CrossFade(this.CooldownAnim, 2.5f);
				this.Timer += Time.deltaTime;
				if (this.Timer > 5f)
				{
					this.Character.GetComponent<Animation>().CrossFade(this.IdleAnim, 1f);
					this.ExpressedSurprise = false;
					this.Threatening = false;
					this.Cooldown = false;
					this.Timer = 0f;
				}
				this.Shove();
			}
		}
		if (Input.GetKeyDown(KeyCode.V) && this.LongSkirt != null)
		{
			this.MyRenderer.sharedMesh = this.LongSkirt;
		}
		if (Input.GetKeyDown(KeyCode.Space) && Vector3.Distance(this.Yandere.transform.position, this.DelinquentManager.transform.position) < 10f)
		{
			this.Spaces++;
			if (this.Spaces == 9)
			{
				if (this.HairRenderer == null)
				{
					this.DefaultHair.SetActive(false);
					this.EasterHair.SetActive(true);
					this.EasterHair.GetComponent<Renderer>().material.mainTexture = this.BlondThugHair;
				}
			}
			else if (this.Spaces == 10)
			{
				this.Rapping = true;
				this.MyWeapon.SetActive(false);
				this.IdleAnim = this.Prefix + "gruntIdle_00";
				Animation component4 = this.Character.GetComponent<Animation>();
				component4.CrossFade(this.IdleAnim);
				component4[this.IdleAnim].time = UnityEngine.Random.Range(0f, component4[this.IdleAnim].length);
				this.DefaultHair.SetActive(false);
				this.Mask.SetActive(false);
				this.EasterHair.SetActive(true);
				this.Bandanas.SetActive(true);
				if (this.HairRenderer != null)
				{
					this.HairRenderer.material.color = this.HairColor;
				}
				this.DelinquentManager.EasterEgg();
			}
		}
		if (this.Suck)
		{
			base.transform.position = Vector3.MoveTowards(base.transform.position, this.TimePortal.position, Time.deltaTime * 10f);
			if (base.transform.position == this.TimePortal.position)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	// Token: 0x06001329 RID: 4905 RVA: 0x000A0454 File Offset: 0x0009E654
	private void Shove()
	{
		if (!this.Yandere.Shoved && !this.Yandere.Tripping && this.DistanceToPlayer < 0.5f)
		{
			AudioSource component = this.DelinquentManager.GetComponent<AudioSource>();
			component.clip = this.ShoveClips[UnityEngine.Random.Range(0, this.ShoveClips.Length)];
			component.Play();
			this.DelinquentManager.SpeechTimer = 5f;
			if (this.Yandere.transform.position.x > base.transform.position.x)
			{
				this.Yandere.transform.position = new Vector3(base.transform.position.x - 0.001f, this.Yandere.transform.position.y, this.Yandere.transform.position.z);
			}
			if (this.Yandere.Aiming)
			{
				this.Yandere.StopAiming();
			}
			Animation component2 = this.Character.GetComponent<Animation>();
			component2[this.ShoveAnim].time = 0f;
			component2.CrossFade(this.ShoveAnim);
			this.Shoving = true;
			this.Yandere.Character.GetComponent<Animation>().CrossFade("f02_shoveA_01");
			this.Yandere.Punching = false;
			this.Yandere.CanMove = false;
			this.Yandere.Shoved = true;
			this.Yandere.ShoveSpeed = 2f;
			this.ExpressedSurprise = false;
			this.Threatening = false;
			this.Cooldown = false;
			this.Timer = 0f;
		}
	}

	// Token: 0x0600132A RID: 4906 RVA: 0x000A0604 File Offset: 0x0009E804
	private void LateUpdate()
	{
		if (!this.Threatening)
		{
			if (!this.Shoving && !this.Rapping)
			{
				this.LookAtTarget = Vector3.Lerp(this.LookAtTarget, this.LookAtPlayer ? this.Yandere.Head.position : this.Default.position, Time.deltaTime * 2f);
				this.Neck.LookAt(this.LookAtTarget);
			}
			if (this.HeadStill)
			{
				this.Head.transform.localEulerAngles = Vector3.zero;
			}
		}
		if (this.BustSize > 0f)
		{
			this.RightBreast.localScale = new Vector3(this.BustSize, this.BustSize, this.BustSize);
			this.LeftBreast.localScale = new Vector3(this.BustSize, this.BustSize, this.BustSize);
		}
	}

	// Token: 0x0600132B RID: 4907 RVA: 0x000A06E9 File Offset: 0x0009E8E9
	private void OnEnable()
	{
		this.Character.GetComponent<Animation>().CrossFade(this.IdleAnim, 1f);
	}

	// Token: 0x0400199E RID: 6558
	private Quaternion targetRotation;

	// Token: 0x0400199F RID: 6559
	public DelinquentManagerScript DelinquentManager;

	// Token: 0x040019A0 RID: 6560
	public YandereScript Yandere;

	// Token: 0x040019A1 RID: 6561
	public Quaternion OriginalRotation;

	// Token: 0x040019A2 RID: 6562
	public Vector3 LookAtTarget;

	// Token: 0x040019A3 RID: 6563
	public GameObject Character;

	// Token: 0x040019A4 RID: 6564
	public SkinnedMeshRenderer MyRenderer;

	// Token: 0x040019A5 RID: 6565
	public GameObject MyWeapon;

	// Token: 0x040019A6 RID: 6566
	public GameObject Jukebox;

	// Token: 0x040019A7 RID: 6567
	public Mesh LongSkirt;

	// Token: 0x040019A8 RID: 6568
	public Camera Eyes;

	// Token: 0x040019A9 RID: 6569
	public Transform RightBreast;

	// Token: 0x040019AA RID: 6570
	public Transform LeftBreast;

	// Token: 0x040019AB RID: 6571
	public Transform Default;

	// Token: 0x040019AC RID: 6572
	public Transform Weapon;

	// Token: 0x040019AD RID: 6573
	public Transform Neck;

	// Token: 0x040019AE RID: 6574
	public Transform Head;

	// Token: 0x040019AF RID: 6575
	public Plane[] Planes;

	// Token: 0x040019B0 RID: 6576
	public string CooldownAnim = "f02_idleShort_00";

	// Token: 0x040019B1 RID: 6577
	public string ThreatenAnim = "f02_threaten_00";

	// Token: 0x040019B2 RID: 6578
	public string SurpriseAnim = "f02_surprise_00";

	// Token: 0x040019B3 RID: 6579
	public string ShoveAnim = "f02_shoveB_00";

	// Token: 0x040019B4 RID: 6580
	public string SwingAnim = "f02_swingA_00";

	// Token: 0x040019B5 RID: 6581
	public string RunAnim = "f02_spring_00";

	// Token: 0x040019B6 RID: 6582
	public string IdleAnim = string.Empty;

	// Token: 0x040019B7 RID: 6583
	public string Prefix = "f02_";

	// Token: 0x040019B8 RID: 6584
	public bool ExpressedSurprise;

	// Token: 0x040019B9 RID: 6585
	public bool LookAtPlayer;

	// Token: 0x040019BA RID: 6586
	public bool Threatening;

	// Token: 0x040019BB RID: 6587
	public bool Attacking;

	// Token: 0x040019BC RID: 6588
	public bool HeadStill;

	// Token: 0x040019BD RID: 6589
	public bool Cooldown;

	// Token: 0x040019BE RID: 6590
	public bool Shoving;

	// Token: 0x040019BF RID: 6591
	public bool Rapping;

	// Token: 0x040019C0 RID: 6592
	public bool Run;

	// Token: 0x040019C1 RID: 6593
	public float DistanceToPlayer;

	// Token: 0x040019C2 RID: 6594
	public float RunSpeed;

	// Token: 0x040019C3 RID: 6595
	public float BustSize;

	// Token: 0x040019C4 RID: 6596
	public float Rotation;

	// Token: 0x040019C5 RID: 6597
	public float Timer;

	// Token: 0x040019C6 RID: 6598
	public int AudioPhase = 1;

	// Token: 0x040019C7 RID: 6599
	public int Spaces;

	// Token: 0x040019C8 RID: 6600
	public AudioClip[] ProximityClips;

	// Token: 0x040019C9 RID: 6601
	public AudioClip[] SurrenderClips;

	// Token: 0x040019CA RID: 6602
	public AudioClip[] SurpriseClips;

	// Token: 0x040019CB RID: 6603
	public AudioClip[] ThreatenClips;

	// Token: 0x040019CC RID: 6604
	public AudioClip[] AggroClips;

	// Token: 0x040019CD RID: 6605
	public AudioClip[] ShoveClips;

	// Token: 0x040019CE RID: 6606
	public AudioClip[] CaseClips;

	// Token: 0x040019CF RID: 6607
	public AudioClip SurpriseClip;

	// Token: 0x040019D0 RID: 6608
	public AudioClip AttackClip;

	// Token: 0x040019D1 RID: 6609
	public AudioClip Crumple;

	// Token: 0x040019D2 RID: 6610
	public AudioClip Strike;

	// Token: 0x040019D3 RID: 6611
	public GameObject DefaultHair;

	// Token: 0x040019D4 RID: 6612
	public GameObject Mask;

	// Token: 0x040019D5 RID: 6613
	public GameObject EasterHair;

	// Token: 0x040019D6 RID: 6614
	public GameObject Bandanas;

	// Token: 0x040019D7 RID: 6615
	public Renderer HairRenderer;

	// Token: 0x040019D8 RID: 6616
	public Color HairColor;

	// Token: 0x040019D9 RID: 6617
	public Texture BlondThugHair;

	// Token: 0x040019DA RID: 6618
	public Transform TimePortal;

	// Token: 0x040019DB RID: 6619
	public bool Suck;
}
