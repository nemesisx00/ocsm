import './Arcana.css'
import React from 'react'
import EntryList from '../../core/EntryList'
import Tracker from '../Tracker'
import { normalizeClassNames, compareStrings } from '../../core/Utilities'

const ArcanaKeys = Object.freeze({
	New: 'new',
	Value: 'value',
	Type: 'type'
})

const ArcanaTypes = Object.freeze({
	Death: 'death',
	Fate: 'fate',
	Forces: 'forces',
	Life: 'life',
	Matter: 'matter',
	Mind: 'mind',
	Prime: 'prime',
	Space: 'space',
	Spirit: 'spirit',
	Time: 'time'
})

class Arcanum extends React.Component
{
	render()
	{
		return (<div className="entry arcanum">
			<select name="arcanaType" value={this.props.type} onChange={(event) => this.props.changeHandler(this.props.index, ArcanaKeys.Type, event.target.value)}>
				<option value="">&nbsp;</option>
				{this.generateOptions()}
			</select>
			<Tracker keyWord={this.props.label} className={normalizeClassNames(`dots long ${this.props.label}`)} type={Tracker.Types.Circle} max={this.props.max} value={this.props.value} changeHandler={(value) => this.props.changeHandler(this.props.index, ArcanaKeys.Value, value)} />
		</div>)
	}
	
	generateOptions()
	{
		let out = []
		Object.keys(ArcanaTypes).forEach((key, i) => {
			out.push((<option key={`option-${i}`} value={ArcanaTypes[key]}>{key}</option>))
		})
		
		return out
	}
}

class Arcana extends EntryList
{
	generateEntries()
	{
		let entries = []
		
		this.props.arcana.sort((a, b) => compareStrings(a.type, b.type))
		this.props.arcana.forEach((arcana, i) => {
			entries.push(<Arcanum key={`Arcana-${i}`} index={i} type={arcana.type} value={arcana.value} max={this.props.max} changeHandler={(index, key, value) => this.props.changeHandler(index, key, value)} />)
		})
		
		return entries
	}
	
	addNewEntry()
	{
		this.props.changeHandler(ArcanaKeys.New, ArcanaKeys.Label, '')
	}
}

Arcana.Keys = ArcanaKeys
Arcana.Types = ArcanaTypes

export default Arcana
